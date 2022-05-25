using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Playground.Dots
{
    public partial class BattleSystem : SystemBase
    {
        private StepPhysicsWorld stepPhysicsWorld;
        private EndSimulationEntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((
                ref BattleData battleData) => {
                    if (battleData.BattleTimer > 0f)
                        battleData.BattleTimer -= deltaTime;
                    if (battleData.TimeoutTimer > 0f)
                        battleData.TimeoutTimer -= deltaTime;
                }).Schedule();
    
            TriggerJob triggerJob = new TriggerJob
            {
                entityCommandBuffer = commandBufferSystem.CreateCommandBuffer(),
                battlers = GetComponentDataFromEntity<BattleData>(),
                translations = GetComponentDataFromEntity<Translation>()
            };

            Dependency = triggerJob.Schedule(stepPhysicsWorld.Simulation, Dependency);
            commandBufferSystem.AddJobHandleForProducer(Dependency);
        }
        
        private struct TriggerJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<BattleData> battlers;
            [ReadOnly] public ComponentDataFromEntity<Translation> translations;

            public EntityCommandBuffer entityCommandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (battlers.HasComponent(triggerEvent.EntityB))
                {
                    if (battlers[triggerEvent.EntityB].TimeoutTimer <= 0f)
                    {
                        entityCommandBuffer.SetComponent(triggerEvent.EntityA, new BattleData { 
                            BattleTimer = 1f,
                            TimeoutTimer = 5f,
                            Target = translations[triggerEvent.EntityB].Value
                        });
                    }
                }

                if (battlers.HasComponent(triggerEvent.EntityA))
                {
                    if (battlers[triggerEvent.EntityA].TimeoutTimer <= 0f)
                    {
                        entityCommandBuffer.SetComponent(triggerEvent.EntityB, new BattleData { 
                            BattleTimer = 1f,
                            TimeoutTimer = 5f,
                            Target = translations[triggerEvent.EntityA].Value
                        });
                    }
                }
            }
        }
    }
}
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
            TriggerJob triggerJob = new TriggerJob
            {
                entityCommandBuffer = commandBufferSystem.CreateCommandBuffer(),
                translations = GetComponentDataFromEntity<Translation>()
            };

            Dependency = triggerJob.Schedule(stepPhysicsWorld.Simulation, Dependency);
            commandBufferSystem.AddJobHandleForProducer(Dependency);
        }
        
        private struct TriggerJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<Translation> translations;

            public EntityCommandBuffer entityCommandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (translations.HasComponent(triggerEvent.EntityB))
                {
                    entityCommandBuffer.SetComponent(triggerEvent.EntityA, new BattleData { 
                        BattleTimer = 1f,
                        Target = translations[triggerEvent.EntityB].Value
                    });
                }
            }
        }
    }
}
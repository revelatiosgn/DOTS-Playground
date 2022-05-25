using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

namespace Playground.Dots
{
    public partial class MoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((
                ref Translation translation, 
                ref Rotation rotation, 
                ref MoveData moveData,
                ref BattleData battleData) => {

                if (battleData.BattleTimer >= 0f)
                {
                    battleData.BattleTimer -= deltaTime;
                    rotation.Value = quaternion.LookRotation(battleData.Target - translation.Value, math.up());
                    return;
                }

                if (moveData.WaitTimer >= 0f)
                {
                    moveData.Speed = 0f;
                    moveData.WaitTimer -= deltaTime;
                    return;
                }

                UnityEngine.Vector3 pos = translation.Value;
                UnityEngine.Vector3 dst = moveData.Target;

                if (UnityEngine.Vector3.SqrMagnitude(pos - dst) < 0.01f)
                {
                    moveData.Target = moveData.NextTarget;
                    moveData.WaitTimer = 2f;
                    return;
                }

                moveData.Speed = moveData.MaxSpeed;
                UnityEngine.Vector3 newPos = UnityEngine.Vector3.MoveTowards(pos, dst, moveData.Speed * deltaTime);
                translation.Value = newPos;
                rotation.Value = quaternion.LookRotation(moveData.Target - translation.Value, math.up());

            }).Schedule();
        }
    }
}
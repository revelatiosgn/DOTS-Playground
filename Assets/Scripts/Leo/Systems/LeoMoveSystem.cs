using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Playground.Leo
{
    public class LeoMoveSystem : IEcsRunSystem
    {
        private readonly EcsPoolInject<LeoMoveData> _movePool = default;
        private readonly EcsFilterInject<Inc<LeoMoveData>> _filter = default;

        public void Run(EcsSystems systems)
        {
            float deltaTime = Time.deltaTime;

            foreach (var entity in _filter.Value)
            {
                ref var moveData = ref _movePool.Value.Get(entity);

                UnityEngine.Vector3 pos = moveData.Transform.position;
                UnityEngine.Vector3 dst = moveData.Target;

                if (moveData.WaitTimer > 0f)
                {
                    moveData.Speed = 0f;
                    moveData.WaitTimer -= deltaTime;
                    continue;
                }

                if (UnityEngine.Vector3.SqrMagnitude(pos - dst) < 0.01f)
                {
                    moveData.Target = moveData.NextTarget;
                    moveData.WaitTimer = 2f;
                    continue;
                }

                moveData.Speed = moveData.MaxSpeed;
                Vector3 newPos = Vector3.MoveTowards(pos, dst, moveData.Speed * deltaTime);
                moveData.Transform.position = newPos;
                moveData.Transform.rotation = Quaternion.LookRotation(moveData.Target - moveData.Transform.position, Vector3.up);
            }
        }
    }
}

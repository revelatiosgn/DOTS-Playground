using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using GPUInstance;

namespace Playground.Leo
{
    public class LeoAnimationSystem : IEcsRunSystem
    {
        
        private readonly EcsPoolInject<LeoAnimationData> _animationPool = default;
        private readonly EcsPoolInject<LeoMoveData> _movePool = default;
        private readonly EcsFilterInject<Inc<LeoAnimationData, LeoMoveData>> _filter = default;

        public void Run(EcsSystems systems)
        {
            float deltaTime = Time.deltaTime;

            foreach (var entity in _filter.Value)
            {
                ref var animationData = ref _animationPool.Value.Get(entity);
                ref var moveData = ref _movePool.Value.Get(entity);

                animationData.skinnedMesh.mesh.position = moveData.Transform.position;
                animationData.skinnedMesh.mesh.rotation = moveData.Transform.rotation;
                animationData.skinnedMesh.mesh.DirtyFlags = DirtyFlag.Position | DirtyFlag.Rotation;

                // if (battleData.BattleTimer > 0f)
                // {
                //     animationData.SetAnimation(animationData.punchAnimID);
                // }
                if (moveData.WaitTimer > 0f)
                {
                    animationData.SetAnimation(animationData.idleAnimID);
                }
                else
                {
                    animationData.SetAnimation(animationData.walkAnimID);
                }
                
                MeshInstancer.Instance.mesh.Set(animationData.skinnedMesh.mesh);
            }
            
            Ticks.GlobalTimeSpeed = 1.0;
            MeshInstancer.Instance.Update(deltaTime);
        }
    }
}

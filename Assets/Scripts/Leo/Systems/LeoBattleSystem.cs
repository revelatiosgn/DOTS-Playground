using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Playground.Leo
{
    public class LeoBattleSystem : IEcsRunSystem
    {
        private readonly EcsPoolInject<LeoHitData> _hitPool = default;
        private readonly EcsFilterInject<Inc<LeoHitData>> _filter = default;

        public void Run(EcsSystems systems)
        {
            // float deltaTime = Time.deltaTime;

            // foreach (var entity in _filter.Value)
            // {
            //     ref var hitData = ref _hitPool.Value.Get(entity);

            //     systems.GetWorld().DelEntity(entity);
            // }
        }
    }
}

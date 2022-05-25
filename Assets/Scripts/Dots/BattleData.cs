using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Playground.Dots
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct BattleData : IComponentData
    {
        public float BattleTimer;
        public float TimeoutTimer;
        public float3 Target;
    }
}

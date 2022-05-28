using Unity.Entities;
using Unity.Mathematics;

namespace Playground.Dots
{
    [System.Serializable]
    public struct MoveData : IComponentData
    {
        public float MaxSpeed;
        public float Speed;
        public float3 Target;
        public float3 MinRange;
        public float3 MaxRange;
        public Random Random;
        public float3 NextTarget => Random.NextFloat3(MinRange, MaxRange);
        public float WaitTimer;
    }
}
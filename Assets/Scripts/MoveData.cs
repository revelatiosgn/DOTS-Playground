using Unity.Entities;
using Unity.Mathematics;

[System.Serializable]
public struct MoveData : IComponentData
{
    public float MaxSpeed;
    public float Speed;
    public float3 Target;
    public float3 Min;
    public float3 Max;
    public Random Random;
    public float3 NextTarget => Random.NextFloat3(Min, Max);
    public float WaitTimer;
}
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct BattleData : IComponentData
{
    public float BattleTimer;
    public float3 Target;
}

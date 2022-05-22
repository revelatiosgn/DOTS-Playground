using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using GPUInstance;
using UnityEngine;

public partial class AnimationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref AnimationData animationData, 
            in Translation translation, 
            in Rotation rotation, 
            in MoveData moveData,
            in BattleData battleData) => 
        {

            animationData.position = translation.Value;
            animationData.rotation = rotation.Value;
            animationData.DirtyFlags = DirtyFlag.Position | DirtyFlag.Rotation;
            
            if (battleData.BattleTimer > 0f)
            {
                animationData.SetAnimation(animationData.punchAnimID);
            }
            else if (moveData.WaitTimer > 0f)
            {
                animationData.SetAnimation(animationData.idleAnimID);
            }
            else
            {
                animationData.SetAnimation(animationData.walkAnimID);
            }

            MeshInstancer.Instance.mesh.Set(animationData);
        }).WithoutBurst().Run();

        Ticks.GlobalTimeSpeed = 1.0;
        var meshInstancer = MeshInstancer.Instance;
        meshInstancer.FrustumCamera = DotsSceneSpawner.Instance.FrustumCullingCamera;
        meshInstancer.Update(Time.DeltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPUInstance;

namespace Playground.Leo
{
    public struct LeoAnimationData
    {
        public SkinnedMesh skinnedMesh;

        public int walkAnimID;
        public int idleAnimID;
        public int punchAnimID;

        public void SetAnimation(int animID)
        {
            if (skinnedMesh.mesh.props_animationID != animID)
            {
                skinnedMesh.mesh.props_animationID = animID;
                skinnedMesh.mesh.DirtyFlags |= DirtyFlag.props_AnimationID;
            }
        }
    }
}

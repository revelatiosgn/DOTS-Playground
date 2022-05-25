using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using GPUInstance;

namespace Playground.Dots
{
    [Serializable]
    public struct AnimationData : IComponentData
    {
        public float3 position;
        public quaternion rotation;
        public float3 scale;
        public int id;
        public ushort groupID;
        public int data1;
        public int parentID;
        public int props_color;
        public float2 props_offset;
        public float2 props_tiling;
        public int propertyID;
        public int props_extra;
        public int props_animationID;
        public int skeletonID;
        public uint props_instanceTicks;
        public int DirtyFlags;
        public int props_pathID;
        public int props_pad2;
        public uint props_pathInstanceTicks;
        public bool HasProperties;
        public int data2;

        public int walkAnimID;
        public int idleAnimID;
        public int punchAnimID;

        public void SetAnimation(int animID)
        {
            if (props_animationID != animID)
            {
                props_animationID = animID;
                DirtyFlags |= DirtyFlag.props_AnimationID;
            }
        }
    }
}


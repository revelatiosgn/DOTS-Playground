using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using GPUInstance;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Playground
{
    public abstract class BaseSpawner : MonoBehaviour, IDisposable
    {
        [SerializeField] protected List<GPUSkinnedMeshComponent> _characters;

        protected string[] _animNames = new string[] {
            "walk", 
            "Mutant Run", 
            "Mutant Breathing Idle", 
            "Mutant Punch", 
            "Mutant Walking"
        };

        protected SkinnedMesh[] _skinnedMeshes;

        private void Start()
        {
            if (!MeshInstancer.Instance.Initialized())
            {
                int hierarchy_depth, skeleton_bone_count;
                var controllers = GPUSkinnedMeshComponent.PrepareControllers(_characters, out hierarchy_depth, out skeleton_bone_count);
                var meshInstancer = MeshInstancer.Instance;
                meshInstancer.Initialize(max_parent_depth: hierarchy_depth + 2, num_skeleton_bones: skeleton_bone_count, pathCount: 2);
                
                meshInstancer.SetAllAnimations(controllers);
                foreach (var character in _characters)
                    meshInstancer.AddGPUSkinnedMeshType(character);
            }

            Init();
        }

        private void OnDestroy()
        {
            Clear();
            Dispose();
        }

        public abstract void Init();
        public abstract void Dispose();
        public abstract void Spawn(SpawnSettings settings);
        public abstract void Clear();

        public struct SpawnSettings
        {
            public int Count;
            public int Dist;
            public float BotSpeed;
        }

        protected void InitMeshInstancer()
        {
        }
    }
}
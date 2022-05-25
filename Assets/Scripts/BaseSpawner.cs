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

        private void Start()
        {
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
    }
}
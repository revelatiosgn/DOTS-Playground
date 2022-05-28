using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPUInstance;
using System;
using System.IO;

namespace Playground.Mono
{
    public class MonoSpawner : BaseSpawner
    {
        [SerializeField] protected MonoBot _botPrefab;

        public override void Init()
        {
            int hierarchy_depth, skeleton_bone_count;
            var controllers = GPUSkinnedMeshComponent.PrepareControllers(_characters, out hierarchy_depth, out skeleton_bone_count);

            var meshInstancer = MeshInstancer.Instance;
            if (meshInstancer.Initialized())
                return;

            meshInstancer.Initialize(max_parent_depth: hierarchy_depth + 2, num_skeleton_bones: skeleton_bone_count, pathCount: 2);
            meshInstancer.SetAllAnimations(controllers);

            foreach (var character in _characters)
                meshInstancer.AddGPUSkinnedMeshType(character);
        }

        public override void Dispose()
        {
        }
        
        public override void Spawn(SpawnSettings settings)
        {
            Clear();
            
            _skinnedMeshes = new SkinnedMesh[settings.Count];

            int current = 0;
            while (current < settings.Count)
            {
                var mesh = _characters[UnityEngine.Random.Range(0, _characters.Count)];
                // var mesh = characters[0];
                // var animName = animNames[UnityEngine.Random.Range(0, animNames.Length)];
                // var anim = mesh.anim.namedAnimations[animName];
                // var anim = mesh.anim.namedAnimations["walk"];
                var anim = mesh.anim.namedAnimations["Mutant Breathing Idle"];
                var skinnedMesh = new SkinnedMesh(mesh, MeshInstancer.Instance);
                skinnedMesh.mesh.position = MonoMover.GetRand(settings.Dist);
                skinnedMesh.SetRadius(1.75f); // set large enough radius so model doesnt get culled to early
                skinnedMesh.Initialize();
                skinnedMesh.SetAnimation(anim, speed: settings.BotSpeed * .5f, start_time: 0f); // set animation
                skinnedMesh.UpdateAll();
                _skinnedMeshes[current] = skinnedMesh;

                MonoBot bot = Instantiate(_botPrefab, skinnedMesh.mesh.position, Quaternion.identity);
                bot.Init(new MonoMover.MoverData{
                            Speed = 0f,
                            MaxSpeed = settings.BotSpeed,
                            Radius = settings.Dist
                        },
                        new MonoAnimator.AnimData {
                            walkAnimID = mesh.anim.namedAnimations["walk"].GPUAnimationID,
                            idleAnimID = mesh.anim.namedAnimations["Mutant Breathing Idle"].GPUAnimationID,
                            punchAnimID = mesh.anim.namedAnimations["Mutant Punch"].GPUAnimationID
                        },
                        skinnedMesh);

                current++;
            }
        }

        public override void Clear()
        {
            if (_skinnedMeshes == null)
                return;

            UnityEngine.Debug.Log($"Clear {_skinnedMeshes.Length}");
            for (int i = 0; i < _skinnedMeshes.Length; i++)
            {
                _skinnedMeshes[i].Dispose();
            }
            _skinnedMeshes = null;
        }

        private void Update()
        {
            Ticks.GlobalTimeSpeed = 1.0;
            var meshInstancer = MeshInstancer.Instance;
            meshInstancer.Update(Time.deltaTime);
        }
    }
}



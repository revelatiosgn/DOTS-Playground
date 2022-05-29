using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using GPUInstance;
using UnityEngine;
using System;

namespace Playground.Dots
{
    public class DotsSpawner : BaseSpawner
    {
        [SerializeField] protected GameObject _botPrefab;

        private EntityManager _manager;
        private BlobAssetStore _blobAssetStore;
        private Entity _entityPrefab;

        private Entity[] _entities;

        public override void Init()
        {
            _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _blobAssetStore = new BlobAssetStore();
            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
            _entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_botPrefab, settings);
        }

        public override void Dispose()
        {
            _blobAssetStore.Dispose();
        }
        
        public override void Spawn(SpawnSettings settings)
        {
            Clear();

            int current = 0;

            _entities = new Entity[settings.Count];
            float dist = Mathf.Sqrt(settings.Count) / settings.Density;

            _skinnedMeshes = new SkinnedMesh[settings.Count];

            while (current < settings.Count)
            {
                Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint) (current + 1));
                MoveData moveData = new MoveData();
                moveData.MaxSpeed = settings.BotSpeed;
                moveData.Speed = 0f;
                moveData.MinRange = new float3(-dist * .5f, 0f, -dist * .5f);
                moveData.MaxRange = new float3(dist * .5f, 0f, dist * .5f);
                moveData.Random = random;
                var t = moveData.NextTarget;
                moveData.Target = moveData.NextTarget;

                var mesh = _characters[UnityEngine.Random.Range(0, _characters.Count)];
                // var mesh = characters[0];
                // var animName = animNames[UnityEngine.Random.Range(0, animNames.Length)];
                // var anim = mesh.anim.namedAnimations[animName];
                // var anim = mesh.anim.namedAnimations["walk"];
                var anim = mesh.anim.namedAnimations["Mutant Breathing Idle"];
                var skinnedMesh = new SkinnedMesh(mesh, MeshInstancer.Instance);
                skinnedMesh.mesh.position = moveData.Target;
                skinnedMesh.SetRadius(1.75f); // set large enough radius so model doesnt get culled to early
                skinnedMesh.Initialize();
                skinnedMesh.SetAnimation(anim, speed: settings.BotSpeed * .5f, start_time: 0f); // set animation
                skinnedMesh.UpdateAll();

                _skinnedMeshes[current] = skinnedMesh;

                Entity newEntity = _manager.Instantiate(_entityPrefab);
                
                _manager.AddComponentData(newEntity, new Translation { 
                    Value = moveData.Target
                });

                _manager.AddComponentData(newEntity, moveData);
                _manager.AddComponentData(newEntity, new BattleData());

                _manager.AddComponentData(newEntity, new AnimationData {
                    position = skinnedMesh.mesh.position,
                    rotation = skinnedMesh.mesh.rotation,
                    scale = skinnedMesh.mesh.scale,
                    id = skinnedMesh.mesh.id,
                    groupID = skinnedMesh.mesh.groupID,
                    data1 = skinnedMesh.mesh.data1,
                    parentID = skinnedMesh.mesh.parentID,
                    props_color = skinnedMesh.mesh.props_color,
                    props_offset = skinnedMesh.mesh.props_offset,
                    props_tiling = skinnedMesh.mesh.props_tiling,
                    propertyID = skinnedMesh.mesh.propertyID,
                    props_extra = skinnedMesh.mesh.props_extra,
                    props_animationID = skinnedMesh.mesh.props_animationID,
                    skeletonID = skinnedMesh.mesh.skeletonID,
                    props_instanceTicks = skinnedMesh.mesh.props_instanceTicks,
                    DirtyFlags = skinnedMesh.mesh.DirtyFlags,
                    props_pathID = skinnedMesh.mesh.props_pathID,
                    props_pad2 = skinnedMesh.mesh.props_pad2,
                    props_pathInstanceTicks = skinnedMesh.mesh.props_pathInstanceTicks,
                    HasProperties = skinnedMesh.mesh.HasProperties,
                    data2 = skinnedMesh.mesh.data2,

                    walkAnimID = mesh.anim.namedAnimations["walk"].GPUAnimationID,
                    idleAnimID = mesh.anim.namedAnimations["Mutant Breathing Idle"].GPUAnimationID,
                    punchAnimID = mesh.anim.namedAnimations["Mutant Punch"].GPUAnimationID
                });

                _entities[current] = newEntity;

                current++;
            }

            MeshInstancer.Instance.FrustumCamera = Camera.main;
        }

        public override void Clear()
        {
            if (_entities == null)
                return;

            UnityEngine.Debug.Log($"Clear {_entities.Length}");
            for (int i = 0; i < _entities.Length; i++)
            {
                AnimationData animationData = _manager.GetComponentData<AnimationData>(_entities[i]);
                _skinnedMeshes[i].Dispose();
                _manager.DestroyEntity(_entities[i]);
            }
            _entities = null;
        }
    }
}
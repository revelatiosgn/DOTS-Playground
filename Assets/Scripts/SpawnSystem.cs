using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using GPUInstance;

public partial class SpawnSystem : SystemBase
{
    private EntityManager _manager;
    private BlobAssetStore _blobAssetStore;
    private Entity _entityPrefab;

    private Entity[] _entities;

    protected override void OnStartRunning()
    {
        _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
        _entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(DotsSceneSpawner.Instance.BotPrefab, settings);

        int hierarchy_depth, skeleton_bone_count;
        var characters = DotsSceneSpawner.Instance.Characters;
        var controllers = GPUSkinnedMeshComponent.PrepareControllers(characters, out hierarchy_depth, out skeleton_bone_count);

        var meshInstancer = MeshInstancer.Instance;
        meshInstancer.Initialize(max_parent_depth: hierarchy_depth + 2, num_skeleton_bones: skeleton_bone_count, pathCount: 2);
        meshInstancer.SetAllAnimations(controllers);

        foreach (var character in characters)
            meshInstancer.AddGPUSkinnedMeshType(character);
    }

    protected override void OnStopRunning()
    {
        _blobAssetStore.Dispose();
    }

    protected override void OnUpdate()
    {
    }
    
    public void Spawn(SpawnSettings settings)
    {
        Clear();

        string[] animNames = new string[] {
            "walk", 
            "Mutant Run", 
            "Mutant Breathing Idle", 
            "Mutant Punch", 
            "Mutant Walking"
        };

        int current = 0;

        var characters = DotsSceneSpawner.Instance.Characters;
        _entities = new Entity[settings.Count];
        float dist = settings.Dist;

        while (current < settings.Count)
        {
            Random random = new Random((uint) (current + 1));
            MoveData moveData = new MoveData();
            moveData.MaxSpeed = settings.BotSpeed;
            moveData.Speed = 0f;
            moveData.Min = new float3(-dist * .5f, 0f, -dist * .5f);
            moveData.Max = new float3(dist * .5f, 0f, dist * .5f);
            moveData.Random = random;
            var t = moveData.NextTarget;
            moveData.Target = moveData.NextTarget;

            var mesh = characters[UnityEngine.Random.Range(0, characters.Count)];
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

            Entity newEntity = _manager.Instantiate(_entityPrefab);
            
            _manager.AddComponentData(newEntity, new Translation { 
                Value = moveData.Target
            });

            _manager.AddComponentData(newEntity, moveData);

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

            // if (_manager.HasComponent<PhysicsMass>(newEntity))
            // {
            //     PhysicsMass mass = _manager.GetComponentData<PhysicsMass>(newEntity);
            //     mass.InverseInertia[0] = 0f;
            //     mass.InverseInertia[1] = 0f;
            //     mass.InverseInertia[2] = 0f;
            // }

            _entities[current] = newEntity;

            current++;
        }
    }

    public void Clear()
    {
        if (_entities == null)
            return;

        UnityEngine.Debug.Log($"Clear {_entities.Length}");
        for (int i = 0; i < _entities.Length; i++)
        {
            AnimationData animationData = GetComponent<AnimationData>(_entities[i]);
            MeshInstancer.Instance.mesh.Delete(animationData);
            EntityManager.DestroyEntity(_entities[i]);
        }
        _entities = null;
    }

    public struct SpawnSettings
    {
        public int Count;
        public int Dist;
        public float BotSpeed;
    }
}

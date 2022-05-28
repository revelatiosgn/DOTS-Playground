using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;
using GPUInstance;
using Voody.UniLeo.Lite;

namespace Playground.Leo
{
    public class LeoSpawner : BaseSpawner
    {
        [SerializeField] protected LeoBot _botPrefab;
        
        private EcsWorld _world;
        public EcsSystems Systems { get; private set; }

        public override void Init()
        {
            _world = new EcsWorld();
            Systems = new EcsSystems(_world);

            Systems.ConvertScene();
            Systems.Add(new LeoAnimationSystem());
            Systems.Add(new LeoMoveSystem());
            Systems.Inject();
            Systems.Init();
        }

        public override void Dispose()
        {
            if (Systems != null)
            {
                Systems.Destroy();
                Systems = null;
            }
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
        
        public override void Spawn(SpawnSettings settings)
        {
            Clear();

            Debug.Log($"Spawn {settings.Count}!");

            float dist = settings.Dist;

            _skinnedMeshes = new SkinnedMesh[settings.Count];

            int current = 0;
            while (current < settings.Count)
            {
                LeoBot leoBot = Instantiate(_botPrefab);

                LeoMoveData moveData = new LeoMoveData();
                moveData.Transform = leoBot.transform;
                moveData.MaxSpeed = settings.BotSpeed;
                moveData.Speed = 0f;
                moveData.MinRange = -dist * .5f;
                moveData.MaxRange = dist * 5f;
                var t = moveData.NextTarget;
                moveData.Target = moveData.NextTarget;
                leoBot.transform.position = moveData.NextTarget;
                leoBot.MoveProvider.Init(moveData);

                var mesh = _characters[UnityEngine.Random.Range(0, _characters.Count)];
                // var mesh = characters[0];
                // var animName = animNames[UnityEngine.Random.Range(0, animNames.Length)];
                // var anim = mesh.anim.namedAnimations[animName];
                // var anim = mesh.anim.namedAnimations["walk"];
                var anim = mesh.anim.namedAnimations["Mutant Breathing Idle"];
                var skinnedMesh = new SkinnedMesh(mesh, MeshInstancer.Instance);
                skinnedMesh.mesh.position = leoBot.transform.position;
                skinnedMesh.mesh.position = default(Vector3);
                skinnedMesh.SetRadius(1.75f); // set large enough radius so model doesnt get culled to early
                skinnedMesh.Initialize();
                skinnedMesh.SetAnimation(anim, speed: settings.BotSpeed * .5f, start_time: 0f); // set animation
                skinnedMesh.UpdateAll();
                _skinnedMeshes[current] = skinnedMesh;

                LeoAnimationData animationData = new LeoAnimationData();
                animationData.skinnedMesh = skinnedMesh;
                animationData.idleAnimID = mesh.anim.namedAnimations["Mutant Breathing Idle"].GPUAnimationID;
                animationData.punchAnimID = mesh.anim.namedAnimations["Mutant Punch"].GPUAnimationID;
                animationData.walkAnimID = mesh.anim.namedAnimations["walk"].GPUAnimationID;
                leoBot.AnimationProvider.Init(animationData);

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
            
            int[] entities = new int[_world.GetWorldSize()];
            _world.GetAllEntities(ref entities);

            for (int i = 0; i < entities.Length; i++)
                _world.DelEntity(entities[i]);
        }

        private void Update()
        {
            Systems?.Run();
        }
    }
}
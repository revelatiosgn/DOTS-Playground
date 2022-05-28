using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Playground.Leo
{
    public class LeoStartup : MonoBehaviour
    {
        private EcsWorld _world;
        public EcsSystems Systems { get; private set; }

        public void Awake()
        {
            _world = new EcsWorld();
            Systems = new EcsSystems(_world);

            Systems.ConvertScene();
            Systems.Add(new LeoAnimationSystem());
            Systems.Add(new LeoMoveSystem());
            Systems.Inject();
            Systems.Init();
        }

        private void Update()
        {
            Systems?.Run();
        }

        private void OnDestroy()
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
    }
}
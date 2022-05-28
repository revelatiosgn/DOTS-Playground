using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Voody.UniLeo.Lite;

namespace Playground.Leo
{
    public class LeoTrigger : MonoBehaviour
    {
        public EcsWorld ecsWorld { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            // int hit = ecsWorld.NewEntity();

            // EcsPool<LeoHitData> pool = ecsWorld.GetPool<LeoHitData>();
            // var hitComponent = pool.Add(hit);

            // hitComponent.goA = transform.gameObject;
            // hitComponent.goB = other.gameObject;
        }
    }
}

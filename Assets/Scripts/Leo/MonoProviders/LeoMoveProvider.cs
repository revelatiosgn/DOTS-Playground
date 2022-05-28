using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Voody.UniLeo.Lite;

namespace Playground.Leo
{
    public class LeoMoveProvider : MonoProvider<LeoMoveData>
    {
        [SerializeField] private Transform _transform;

        private void Awake()
        {
            value.Transform = _transform;
        }

        public void Init(LeoMoveData moveData)
        {
            value = moveData;
        }
    }
}

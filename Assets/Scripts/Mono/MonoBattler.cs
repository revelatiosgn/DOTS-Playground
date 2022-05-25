using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playground.Mono
{
    public class MonoBattler : MonoBehaviour
    {
        private Vector3 _target;
        private float _battleTimer;

        public Vector3 Target => _target;
        public float BattleTimer => _battleTimer;

        private void Update()
        {
            if (_battleTimer > 0f)
                _battleTimer -= Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            _target = other.transform.position;
            _battleTimer = 1f;
        }
    }
}



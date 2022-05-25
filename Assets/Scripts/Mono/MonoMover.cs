using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playground.Mono
{
    public class MonoMover : MonoBehaviour
    {
        [SerializeField] private MonoBattler _battler;

        private MoverData _data;
        private Vector3 _target;
        private float _waitTimer;

        public float WaitTimer => _waitTimer;

        public void Init(MoverData data)
        {
            _data = data;
        }

        private void Start()
        {
            _target = GetRand(_data.Radius);
        }

        private void Update()
        {
            if (_battler.BattleTimer > 0f)
            {
                transform.rotation = Quaternion.LookRotation(_battler.Target - transform.position, Vector3.up);
                return;
            }

            if (_waitTimer > 0f)
            {
                _waitTimer -= Time.deltaTime;
                return;
            }

            if (Vector3.SqrMagnitude(transform.position - _target) < 0.01f)
            {
                _target = GetRand(_data.Radius);
                _waitTimer = 1f;
                return;
            }

            transform.rotation = Quaternion.LookRotation(_target - transform.position, Vector3.up);
            transform.position = Vector3.MoveTowards(transform.position, _target, _data.MaxSpeed * Time.deltaTime);
        }

        public static Vector3 GetRand(float dist)
        {
            dist *= .5f;
            return new Vector3(
                Random.Range(-dist, dist),
                0f,
                Random.Range(-dist, dist)
            );
        }

        public struct MoverData
        {
            public float MaxSpeed;
            public float Speed;
            public float Radius;
        }
    }
}



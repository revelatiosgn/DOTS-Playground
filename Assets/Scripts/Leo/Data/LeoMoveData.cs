using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playground.Leo
{
    public struct LeoMoveData
    {
        public Transform Transform;
        public float MaxSpeed;
        public float Speed;
        public Vector3 Target;
        public float MinRange;
        public float MaxRange;
        public Vector3 NextTarget => GetRandomVector3(MinRange, MaxRange);
        public float WaitTimer;

        public static Vector3 GetRandomVector3(float min, float max)
        {
            return new Vector3(Random.Range(min, max), 0f, Random.Range(min, max));
        }
    }
}

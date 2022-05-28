using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Voody.UniLeo.Lite;
using GPUInstance;

namespace Playground.Leo
{
    public class LeoAnimationProvider : MonoProvider<LeoAnimationData>
    {
        public void Init(LeoAnimationData data)
        {
            value = data;
        }
    }
}

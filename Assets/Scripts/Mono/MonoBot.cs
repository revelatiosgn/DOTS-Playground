using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPUInstance;

namespace Playground.Mono
{
    public class MonoBot : MonoBehaviour
    {
        [SerializeField] private MonoMover _mover;
        [SerializeField] private MonoAnimator _animator;

        public void Init(MonoMover.MoverData moverData, MonoAnimator.AnimData animData, SkinnedMesh mesh)
        {
            _mover.Init(moverData);
            _animator.Init(mesh, animData);
        }
    }
}



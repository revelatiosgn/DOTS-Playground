using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPUInstance;

namespace Playground.Mono
{
    public class MonoAnimator : MonoBehaviour
    {
        [SerializeField] private MonoMover _mover;
        [SerializeField] private MonoBattler _battler;

        private SkinnedMesh _skinnedMesh;
        private AnimData _animData;

        public AnimData Data => _animData;

        public void Init(SkinnedMesh skinnedMesh, AnimData animData)
        {
            _skinnedMesh = skinnedMesh;
            _animData = animData;

            SetAnimation(_animData.walkAnimID);

            // _skinnedMesh.mesh.position = default(Vector3);
            // _skinnedMesh.mesh.DirtyFlags |= DirtyFlag.Position;
            // _skinnedMesh.mesh.DirtyFlags |= DirtyFlag.Rotation;
            // _skinnedMesh.mesh.DirtyFlags |= DirtyFlag.props_AnimationID;

            // MeshInstancer.Instance.Append(ref _skinnedMesh.mesh);

            // MeshInstancer.Instance.Append(ref _skinnedMesh.mesh);
        }

        private void Update()
        {
            _skinnedMesh.mesh.position = transform.position;
            _skinnedMesh.mesh.DirtyFlags |= DirtyFlag.Position;
            _skinnedMesh.mesh.rotation = transform.rotation;
            _skinnedMesh.mesh.DirtyFlags |= DirtyFlag.Rotation;

            if (_battler.BattleTimer > 0f)
            {
                SetAnimation(_animData.punchAnimID);
            }
            else if (_mover.WaitTimer > 0f)
            {
                SetAnimation(_animData.idleAnimID);
            }
            else
            {
                SetAnimation(_animData.walkAnimID);
            }

            
            MeshInstancer.Instance.Append(ref _skinnedMesh.mesh);
        }
        
        private void SetAnimation(int id)
        {
            if (_skinnedMesh.mesh.props_animationID != id)
            {
                _skinnedMesh.mesh.props_animationID = id;
                _skinnedMesh.mesh.DirtyFlags |= DirtyFlag.props_AnimationID;
            }
        }

        public struct AnimData
        {
            public int walkAnimID;
            public int idleAnimID;
            public int punchAnimID;
        }
    }
}



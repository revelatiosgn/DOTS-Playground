// ----------------------------------------------------------------------------
// The MIT License
// UnityEditor integration https://github.com/Leopotam/ecslite-unityeditor
// for LeoECS Lite https://github.com/Leopotam/ecslite
// Copyright (c) 2021-2022 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors {
    sealed class AnimationCurveInspector : EcsComponentInspectorTyped<AnimationCurve> {
        public override bool OnGuiTyped (string label, ref AnimationCurve value, EcsEntityDebugView entityView) {
            var newValue = EditorGUILayout.CurveField (label, value);
            if (newValue.Equals (value)) { return false; }
            value = newValue;
            return true;
        }
    }
}
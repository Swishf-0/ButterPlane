#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    [CustomEditor(typeof(LetterDataSetEditTool))]
    public class LetterDataSetEditToolEditor : Editor
    {
        LetterDataSetEditTool _target;

        private void OnEnable()
        {
            _target = target as LetterDataSetEditTool;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("整列"))
            {
                _target.Normalize();
                EditorUtility.SetDirty(_target.gameObject);
            }
        }
    }
}
#endif

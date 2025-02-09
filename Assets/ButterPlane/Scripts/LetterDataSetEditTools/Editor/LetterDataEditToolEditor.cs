#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    [CustomEditor(typeof(LetterDataEditTool))]
    public class LetterDataEditToolEditor : Editor
    {
        LetterDataEditTool _target;

        private void OnEnable()
        {
            _target = target as LetterDataEditTool;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("整列"))
            {
                _target.Normalize();
                EditorUtility.SetDirty(_target.gameObject);
            }

            GUILayout.Label("アンカー");
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("左"))
                {
                    _target.SetAnchorLeft();
                    EditorUtility.SetDirty(_target.gameObject);
                }

                if (GUILayout.Button("中央"))
                {
                    _target.SetAnchorCenter();
                    EditorUtility.SetDirty(_target.gameObject);
                }

                if (GUILayout.Button("右"))
                {
                    _target.SetAnchorRight();
                    EditorUtility.SetDirty(_target.gameObject);
                }
            }

            GUILayout.Label("長さ変更");
            {
                _target.LengthUnit = EditorGUILayout.FloatField("長さ", _target.LengthUnit);
                if (GUILayout.Button($"この文字の線の長さを全て {_target.LengthUnit} に変更"))
                {
                    _target.SetLengthToUnit();
                    EditorUtility.SetDirty(_target.gameObject);
                }
            }
        }
    }
}
#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    [CustomEditor(typeof(LineEditTool))]
    public class LineEditToolEditor : Editor
    {
        LineEditTool _target;

        string[] _lengthTabTexts = { "値指定", "割合指定" };
        int _lengthTabIdx;
        int _gridCount;

        private void OnEnable()
        {
            _target = target as LineEditTool;
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

            GUILayout.Label("配置");
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("左"))
                {
                    _target.SetPositionLeft();
                    EditorUtility.SetDirty(_target.gameObject);
                }

                if (GUILayout.Button("中央"))
                {
                    _target.SetPositionCenter();
                    EditorUtility.SetDirty(_target.gameObject);
                }

                if (GUILayout.Button("右"))
                {
                    _target.SetPositionRight();
                    EditorUtility.SetDirty(_target.gameObject);
                }
            }

            EditorGUI.BeginChangeCheck();
            {
                _gridCount = EditorGUILayout.IntSlider("グリッド位置", _gridCount, 0, _target.GridCount + 1);
            }
            var gcChanged = EditorGUI.EndChangeCheck();

            _target.GridCount = EditorGUILayout.IntSlider("グリッド数", _target.GridCount, 1, 10);
            if (GUILayout.Button("グリッドに配置") || gcChanged)
            {
                _target.MoveToGrid(_gridCount);
                EditorUtility.SetDirty(_target.gameObject);
            }

            GUILayout.Label("長さ変更");
            {
                _target.LengthUnit = EditorGUILayout.FloatField("長さ", _target.LengthUnit);
                if (GUILayout.Button($"線の長さを {_target.LengthUnit} に変更"))
                {
                    _target.SetLengthToUnit();
                    EditorUtility.SetDirty(_target.gameObject);
                }

                if (GUILayout.Button($"この行の線の長さを全て {_target.LengthUnit} に変更"))
                {
                    foreach (var tool in _target.GetSiblingLines())
                    {
                        tool.LengthUnit = _target.LengthUnit;
                        tool.SetLengthToUnit();
                    }
                    EditorUtility.SetDirty(_target.gameObject);
                }

                if (GUILayout.Button($"この文字の線の長さを全て {_target.LengthUnit} に変更"))
                {
                    var tool = _target.GetLetterTool();
                    tool.LengthUnit = _target.LengthUnit;
                    tool.SetLengthToUnit();
                    EditorUtility.SetDirty(_target.gameObject);
                }

                // _lengthTabIdx = GUILayout.Toolbar(_lengthTabIdx, _lengthTabTexts, new GUIStyle(EditorStyles.toolbarButton), GUI.ToolbarButtonSize.Fixed);
                // if (_lengthTabIdx == 0)
                // {
                //     _sliderValue = EditorGUILayout.IntSlider(_sliderValue, 0, (int)(LineEditTool.LETTER_WIDTH * 2) * 100);
                //     using (new EditorGUI.DisabledScope(true))
                //     {
                //         _length = _sliderValue * 0.01f;
                //         _length = EditorGUILayout.FloatField("長さ", _length);
                //     }
                // }
                // else if (_lengthTabIdx == 1)
                // {
                //     _sliderValue = EditorGUILayout.IntSlider(_sliderValue, 0, 200);
                //     using (new EditorGUI.DisabledScope(true))
                //     {
                //         _length = LineEditTool.LETTER_WIDTH * _sliderValue * 0.01f;
                //         _length = EditorGUILayout.FloatField("長さ", _length);
                //     }
                // }
            }
        }
    }
}
#endif

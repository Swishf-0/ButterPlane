#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    [CustomEditor(typeof(LetterEffectController))]
    public class LetterEffectControllerEditor : Editor
    {
        LetterEffectController _target;
        SerializedProperty _reverse, _interval, _timeScale, _spaceInterval;

        string[] _letterDirectionOptions = new[] { "上から", "下から" };

        private void OnEnable()
        {
            _target = target as LetterEffectController;

            _reverse = serializedObject.FindProperty("_reverse");
            _interval = serializedObject.FindProperty("_interval");
            _timeScale = serializedObject.FindProperty("_timeScale");
            _spaceInterval = serializedObject.FindProperty("_spaceInterval");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                using (new GUILayout.HorizontalScope())
                {

                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("文字を見る方向", EditorStyles.boldLabel);
                    GUI.color = Color.white;
                    var idx = EditorGUILayout.Popup(_reverse.boolValue ? 1 : 0, _letterDirectionOptions);
                    _reverse.boolValue = idx == 1;
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("文字の間隔", EditorStyles.boldLabel);
                    GUI.color = Color.white;
                    _interval.floatValue = EditorGUILayout.FloatField(_interval.floatValue);
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("文字の幅", EditorStyles.boldLabel);
                    GUI.color = Color.white;
                    _timeScale.floatValue = EditorGUILayout.FloatField(_timeScale.floatValue);
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("スペースの幅", EditorStyles.boldLabel);
                    GUI.color = Color.white;
                    _spaceInterval.floatValue = EditorGUILayout.FloatField(_spaceInterval.floatValue);
                }
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            DrawDefaultInspector();
        }
    }
}
#endif

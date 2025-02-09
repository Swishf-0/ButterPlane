#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    [CustomEditor(typeof(PlaneController))]
    public class PlaneControllerEditor : Editor
    {
        PlaneController _target;
        SerializedProperty _offset, _speed, _course;

        private void OnEnable()
        {
            _target = target as PlaneController;

            _offset = serializedObject.FindProperty("_offset");
            _speed = serializedObject.FindProperty("_speed");
            _course = serializedObject.FindProperty("_course");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("飛行機どうしの間隔", EditorStyles.boldLabel);
                    GUI.color = Color.white;
                    _offset.floatValue = EditorGUILayout.FloatField(_offset.floatValue);
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("飛行機の速さ", EditorStyles.boldLabel);
                    GUI.color = Color.white;
                    _speed.floatValue = EditorGUILayout.FloatField(_speed.floatValue);
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("コース", EditorStyles.boldLabel);
                    GUI.color = Color.white;
                    EditorGUILayout.ObjectField(_course, new GUIContent(""));
                }
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            DrawDefaultInspector();
        }
    }
}
#endif

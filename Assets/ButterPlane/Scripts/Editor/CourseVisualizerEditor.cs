#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    [CustomEditor(typeof(CourseVisualizer))]
    public class CourseVisualizerEditor : Editor
    {
        CourseVisualizer _target;

        private void OnEnable()
        {
            _target = target as CourseVisualizer;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("オブジェクト整理"))
            {
                int i = 0;
                foreach (Transform pointObj in _target.transform)
                {
                    pointObj.name = $"p_{i++}";
                    pointObj.localScale = Vector3.one;
                    pointObj.rotation = Quaternion.identity; ;
                }

                EditorUtility.SetDirty(_target.gameObject);
            }

            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector();
        }
    }
}
#endif

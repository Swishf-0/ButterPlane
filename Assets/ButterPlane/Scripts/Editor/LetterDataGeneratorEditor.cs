#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    [CustomEditor(typeof(LetterDataGenerator))]
    public class LetterDataGeneratorEditor : Editor
    {
        LetterDataGenerator _target;

        private void OnEnable()
        {
            _target = target as LetterDataGenerator;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("データ変換"))
            {
                _target.Generate();
                EditorUtility.SetDirty(_target.gameObject);
            }

            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector();
        }
    }
}
#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    [CustomEditor(typeof(LetterDataSetGenerator))]
    public class LetterDataSetGeneratorEditor : Editor
    {
        LetterDataSetGenerator _target;

        private void OnEnable()
        {
            _target = target as LetterDataSetGenerator;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("データ変換"))
            {
                _target.Generate();
                SaveLetterDataSet(_target.LetterDataSetPrefab, _target.LetterDataTextList, _target.LetterDataNames);

                EditorUtility.SetDirty(_target.gameObject);
            }

            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector();
        }

        static bool SaveLetterDataSet(UnityEngine.Object prefab, string[] letterDataTextList, string[] _letterDataNames)
        {
            var prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
            var contentsRoot = PrefabUtility.LoadPrefabContents(prefabPath);
            if (contentsRoot == null)
            {
                return false;
            }
            var letterDataSet = contentsRoot.GetComponent<LetterDataSet>();
            if (letterDataSet == null)
            {
                return false;
            }

            letterDataSet.LetterDataTextList = letterDataTextList;
            letterDataSet.LetterDataNames = _letterDataNames;

            PrefabUtility.SaveAsPrefabAsset(contentsRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(contentsRoot);

            return true;
        }
    }
}
#endif

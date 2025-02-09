#if UNITY_EDITOR
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    public class LetterDataSetGenerator : MonoBehaviour
    {
        public string[] LetterDataTextList => _letterDataTextList;
        public string[] LetterDataNames => _letterDataNames;
        public Transform LetterDataSetPrefab => _letterDataSetPrefab;

        [SerializeField] Transform _letterDataSetPrefab;
        [SerializeField, TextArea(1, 5)] string[] _letterDataTextList;
        [SerializeField] string[] _letterDataNames;

        public void Generate()
        {
            LetterDataSetObjToText(transform, out _letterDataTextList, out _letterDataNames);
        }

        static void LetterDataSetObjToText(Transform letterDataSetObj, out string[] letterDataTextList, out string[] letterDataNames)
        {
            var generators = letterDataSetObj.GetComponentsInChildren<LetterDataGenerator>();
            letterDataTextList = new string[generators.Length];
            letterDataNames = new string[generators.Length];
            for (int i = 0; i < generators.Length; i++)
            {
                generators[i].Generate();
                letterDataTextList[i] = generators[i].LetterDataText;
                letterDataNames[i] = generators[i].LetterDataName;
            }
        }
    }
}
#endif

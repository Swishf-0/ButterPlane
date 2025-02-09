#if UNITY_EDITOR
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    public class LetterDataGenerator : MonoBehaviour
    {
        public string LetterDataText => _letterDataText;
        public string LetterDataName => _letterDataName;

        [SerializeField, TextArea(1, 5)] string _letterDataText;
        [SerializeField] string _letterDataName;

        public void Generate()
        {
            LetterDataObjToText(transform, out var letterDataStartList, out var letterDataEndList, out _letterDataName);

            _letterDataText = LetterDataUtils.LetterDataToText(letterDataStartList, letterDataEndList, LetterDataSet.DIGIT);
        }

        static void LetterDataObjToText(Transform letterDataObj, out float[][] letterDataStartList, out float[][] letterDataEndList, out string letterDataName)
        {
            letterDataStartList = new float[letterDataObj.childCount][];
            letterDataEndList = new float[letterDataObj.childCount][];

            letterDataName = letterDataObj.name;

            var startX = letterDataObj.position.x;

            for (int i = 0; i < letterDataObj.childCount; i++)
            {
                Transform lineGroupObj = letterDataObj.GetChild(i);

                letterDataStartList[i] = new float[lineGroupObj.childCount];
                letterDataEndList[i] = new float[lineGroupObj.childCount];
                for (int j = 0; j < lineGroupObj.childCount; j++)
                {
                    var lineObj = lineGroupObj.GetChild(j);
                    Transform startObj = lineObj.GetChild(0);
                    Transform endObj = lineObj.GetChild(1);
                    letterDataStartList[i][j] = startObj.position.x - startX;
                    letterDataEndList[i][j] = endObj.position.x - startX;
                }
            }
        }

        void OnDrawGizmos()
        {
            foreach (Transform lineGroupObj in transform)
            {
                foreach (Transform lineObj in lineGroupObj)
                {
                    Gizmos.DrawLine(lineObj.GetChild(0).position, lineObj.GetChild(1).transform.position);
                }
            }
        }
    }
}
#endif

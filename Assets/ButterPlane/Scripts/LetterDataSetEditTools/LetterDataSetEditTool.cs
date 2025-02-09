#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    public class LetterDataSetEditTool : MonoBehaviour
    {
        public void Normalize()
        {
            NormalizeChildComponents();
        }

        void NormalizeChildComponents()
        {
            var letterDataEditTool = GetComponentsInChildren<LetterDataEditTool>();
            foreach (var tool in letterDataEditTool)
            {
                tool.Normalize();
            }
        }
    }
}
#endif

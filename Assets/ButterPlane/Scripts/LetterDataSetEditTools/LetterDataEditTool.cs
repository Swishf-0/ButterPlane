#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    public class LetterDataEditTool : MonoBehaviour
    {
        public float LengthUnit { get => _lengthUnit; set => _lengthUnit = value; }

        [SerializeField] float _lengthUnit = 0.1f;

        public void Normalize()
        {
            NormalizeTransform();
            NormalizeChildComponents();
            NormalizeName();
        }

        void NormalizeChildComponents()
        {
            var lineEditTools = GetComponentsInChildren<LineEditTool>();
            foreach (var tool in lineEditTools)
            {
                tool.Normalize();
            }
        }

        void NormalizeName()
        {
            foreach (Transform lineGroupObj in transform)
            {
                lineGroupObj.name = $"LineGroup_{lineGroupObj.GetSiblingIndex()}";
                foreach (Transform lineObj in lineGroupObj)
                {
                    lineObj.name = $"Line_{lineObj.GetSiblingIndex()}";
                }
            }
        }

        void NormalizeTransform()
        {
            Dictionary<Transform, Vector3> tTable = new Dictionary<Transform, Vector3>();
            foreach (Transform lineGroupObj in transform)
            {
                foreach (Transform lineObj in lineGroupObj)
                {
                    foreach (Transform pointObj in lineObj)
                    {
                        tTable[pointObj] = pointObj.position;
                    }
                }
            }

            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            foreach (Transform lineGroupObj in transform)
            {
                lineGroupObj.localRotation = Quaternion.identity;
                lineGroupObj.localScale = Vector3.one;
                foreach (Transform lineObj in lineGroupObj)
                {
                    lineObj.localRotation = Quaternion.identity;
                    lineObj.localScale = Vector3.one;
                }
            }

            float DISTANCE = 0.25f;
            foreach (Transform lineGroupObj in transform)
            {
                var posY = (transform.childCount - lineGroupObj.GetSiblingIndex() - 1) * DISTANCE;
                lineGroupObj.localPosition = new Vector3(0, posY, 0);
                foreach (Transform lineObj in lineGroupObj)
                {
                    MiscUtils.SetLocalPositionY(lineObj, 0);
                    foreach (Transform pointObj in lineObj)
                    {
                        var p = tTable[pointObj];
                        p.y = lineGroupObj.position.y;
                        tTable[pointObj] = p;
                    }
                }
            }

            foreach (Transform t in tTable.Keys)
            {
                t.position = tTable[t];
            }

            // foreach (Transform lineGroupObj in transform)
            // {
            //     var p = point.position;
            //     p.y = posY;
            //     tTable[point] = p;
            // }


            // var posY = _points[0].position.y;
            // var pPos = transform.position;
            // pPos.y = posY;
            // Dictionary<Transform, Vector3> tTable = new Dictionary<Transform, Vector3>();
            // foreach (Transform point in _points)
            // {
            //     var p = point.position;
            //     p.y = posY;
            //     tTable[point] = p;
            // }
        }

        public void SetAnchorLeft()
        {
            var lineEditTools = GetComponentsInChildren<LineEditTool>();
            foreach (var tool in lineEditTools)
            {
                tool.SetAnchorLeft();
            }
        }

        public void SetAnchorCenter()
        {
            var lineEditTools = GetComponentsInChildren<LineEditTool>();
            foreach (var tool in lineEditTools)
            {
                tool.SetAnchorCenter();
            }
        }

        public void SetAnchorRight()
        {
            var lineEditTools = GetComponentsInChildren<LineEditTool>();
            foreach (var tool in lineEditTools)
            {
                tool.SetAnchorRight();
            }
        }

        public void SetLengthToUnit()
        {
            var lineEditTools = GetComponentsInChildren<LineEditTool>();
            foreach (var tool in lineEditTools)
            {
                tool.LengthUnit = LengthUnit;
                tool.SetLengthToUnit();
            }
        }
    }
}
#endif

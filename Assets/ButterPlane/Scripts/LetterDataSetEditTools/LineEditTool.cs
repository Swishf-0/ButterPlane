#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Swishf.ButterPlane.EditorScript
{
    public class LineEditTool : MonoBehaviour
    {
        const int CHILD_COUNT = 2;
        const float POINT_SCALE = 0.05f;
        public const float LETTER_WIDTH = 1;

        public float LengthUnit { get => _lengthUnit; set => _lengthUnit = value; }
        public int GridCount { get => _gridCount; set => _gridCount = value; }

        [SerializeField] float _lengthUnit = 0.1f;
        [SerializeField] int _gridCount = 5;

        Transform[] _points = new Transform[CHILD_COUNT];

        void GetPoints()
        {
            for (int i = 0; i < CHILD_COUNT; i++)
            {
                _points[i] = transform.GetChild(i);
            }
        }

        Transform GetLetterObj()
        {
            return transform.parent.parent;
        }

        public LetterDataEditTool GetLetterTool()
        {
            return GetLetterObj().GetComponent<LetterDataEditTool>();
        }

        public void Normalize()
        {
            GetPoints();
            NormalizeOrder();
            NormalizeName();
            NormalizeTransform();
        }

        void NormalizeName()
        {
            _points[0].name = "p_s";
            _points[1].name = "p_e";
        }

        Dictionary<Transform, Vector3> KeepPointPositions()
        {
            Dictionary<Transform, Vector3> tTable = new Dictionary<Transform, Vector3>();
            foreach (Transform point in _points)
            {
                tTable[point] = point.position;
            }
            return tTable;
        }

        void RestorePointPositions(Dictionary<Transform, Vector3> tTable)
        {
            foreach (Transform t in tTable.Keys)
            {
                t.position = tTable[t];
            }
        }

        void NormalizeTransform()
        {
            GetPoints();
            var posY = _points[0].position.y;
            var pPos = transform.position;
            pPos.y = posY;
            Dictionary<Transform, Vector3> tTable = new Dictionary<Transform, Vector3>();
            foreach (Transform point in _points)
            {
                var p = point.position;
                p.y = posY;
                tTable[point] = p;
            }

            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            transform.position = pPos;
            MiscUtils.SetLocalPositionZ(transform, 0);

            foreach (Transform t in tTable.Keys)
            {
                t.localRotation = Quaternion.identity;
            }

            foreach (Transform point in _points)
            {
                point.localScale = POINT_SCALE * Vector3.one;
            }

            foreach (Transform t in tTable.Keys)
            {
                t.position = tTable[t];
                MiscUtils.SetLocalPositionZ(t, 0);
            }
        }

        void NormalizeOrder()
        {
            if (_points[0].position.x < _points[1].position.x)
            {
                return;
            }

            _points[1].SetSiblingIndex(0);

            var p0 = _points[0];
            var p1 = _points[1];
            _points[0] = p1;
            _points[1] = p0;
        }

        public void SetAnchorLeft()
        {
            GetPoints();
            var tTable = KeepPointPositions();
            transform.position = _points[0].position;
            RestorePointPositions(tTable);
        }

        public void SetAnchorCenter()
        {
            GetPoints();
            var tTable = KeepPointPositions();
            transform.position = (_points[0].position + _points[1].position) / 2;
            RestorePointPositions(tTable);
        }

        public void SetAnchorRight()
        {
            GetPoints();
            var tTable = KeepPointPositions();
            transform.position = _points[1].position;
            RestorePointPositions(tTable);
        }

        public void SetPositionLeft()
        {
            GetPoints();
            var letterObj = GetLetterObj();
            var baseLeftPosX = letterObj.position.x;
            var offsetX = baseLeftPosX - _points[0].position.x;
            MiscUtils.SetPositionX(transform, transform.position.x + offsetX);
        }

        public void SetPositionCenter()
        {
            GetPoints();
            var letterObj = GetLetterObj();
            var baseCenterPosX = letterObj.position.x + LETTER_WIDTH / 2;
            var pointCenterPosX = (_points[0].position.x + _points[1].position.x) / 2;
            var offsetX = baseCenterPosX - pointCenterPosX;
            MiscUtils.SetPositionX(transform, transform.position.x + offsetX);
        }

        public void SetPositionRight()
        {
            GetPoints();
            var letterObj = GetLetterObj();
            var baseRightPosX = letterObj.position.x + LETTER_WIDTH;
            var offsetX = baseRightPosX - _points[1].position.x;
            MiscUtils.SetPositionX(transform, transform.position.x + offsetX);
        }

        public void SetLengthToUnit()
        {
            SetLength(_lengthUnit);
        }

        public void SetLength(float length)
        {
            GetPoints();
            var x = transform.position.x;
            var p0_x = _points[0].position.x;
            var p1_x = _points[1].position.x;
            var l = p1_x - p0_x;
            var diff_l = length - l;
            if (x <= p0_x)
            {
                MiscUtils.SetPositionX(_points[1], _points[1].position.x + diff_l);
                return;
            }

            if (p1_x <= x)
            {
                MiscUtils.SetPositionX(_points[0], _points[0].position.x - diff_l);
                return;
            }

            var r0 = (x - p0_x) / l;
            var r1 = (p1_x - x) / l;
            MiscUtils.SetPositionX(_points[0], _points[0].position.x - diff_l * r0);
            MiscUtils.SetPositionX(_points[1], _points[1].position.x + diff_l * r1);
        }

        public LineEditTool[] GetSiblingLines()
        {
            return transform.parent.GetComponentsInChildren<LineEditTool>();
        }

        public void MoveToGrid(int gridNo)
        {
            GetPoints();
            var letterObj = GetLetterObj();
            var baseLeftPosX = letterObj.position.x;
            MiscUtils.SetPositionX(transform, baseLeftPosX + gridNo / (float)_gridCount);
        }
    }
}
#endif

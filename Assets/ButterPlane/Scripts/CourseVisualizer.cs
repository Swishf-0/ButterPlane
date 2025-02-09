#if UNITY_EDITOR
using UnityEngine;
using System.Reflection;

namespace Swishf.ButterPlane
{
    public class CourseVisualizer : MonoBehaviour
    {
        [SerializeField] bool _showLine = true;
        [SerializeField] SplineData _splineData;
        [SerializeField] Transform _courceAnchorRoot;
        [SerializeField] float _drawStep = 0.05f;
        [SerializeField] PlaneController _planeController;

        Vector3[] _anchorPoints, _drawStepPositions, _drawStepPositionsLeft, _drawStepPositionsRight;

        void Initialize()
        {
            _anchorPoints = MiscUtils.GetChildrenPositions(_courceAnchorRoot, Vector3.zero);
            _splineData.Initialize(ref _anchorPoints);
            _drawStepPositions = new Vector3[(int)(_splineData.GetPointCount() * (1 / _drawStep)) + 2];
            for (int i = 0; i < _drawStepPositions.Length - 1; i++)
            {
                _drawStepPositions[i] = _splineData.CalcPosition(i * _drawStep);
            }
            _drawStepPositions[_drawStepPositions.Length - 1] = _splineData.CalcPosition(0);

            if (_planeController != null)
            {
                FieldInfo fieldInfo = typeof(PlaneController).GetField("_planes", BindingFlags.NonPublic | BindingFlags.Instance);
                var _planes = fieldInfo.GetValue(_planeController) as Transform[];
                if (_planes == null || _planes.Length <= 1)
                {
                    return;
                }

                fieldInfo = typeof(PlaneController).GetField("_offset", BindingFlags.NonPublic | BindingFlags.Instance);
                var _offset = (float)fieldInfo.GetValue(_planeController);

                _drawStepPositionsLeft = new Vector3[_drawStepPositions.Length];
                _drawStepPositionsRight = new Vector3[_drawStepPositions.Length];
                for (int i = 0; i < _drawStepPositions.Length; i++)
                {
                    var preIdx = i == 0 ? _drawStepPositions.Length - 2 : i - 1;
                    var _prePos = _drawStepPositions[preIdx];
                    var pos = _drawStepPositions[i];
                    var nextIdx = i == _drawStepPositions.Length - 1 ? 1 : i + 1;
                    var nextPos = _drawStepPositions[nextIdx];
                    var dirNextPre = nextPos - _prePos;
                    var dirOffsetRight = -Vector3.Cross(dirNextPre, Vector3.up).normalized;

                    float offset = (_planes.Length - 1) / 2f;
                    int _i = 0;
                    _drawStepPositionsLeft[i] = pos - (offset - _i) * _offset * dirOffsetRight;
                    _i = _planes.Length - 1;
                    _drawStepPositionsRight[i] = pos - (offset - _i) * _offset * dirOffsetRight;
                }
            }
        }

        void OnDrawGizmos()
        {
            if (!_showLine)
            {
                return;
            }

            Initialize();

            DrawCourse(ref _anchorPoints, ref _drawStepPositions, ref _drawStepPositionsLeft, ref _drawStepPositionsRight);
        }

        static void DrawCourse(ref Vector3[] points, ref Vector3[] drawStepPositions, ref Vector3[] _drawStepPositionsLeft, ref Vector3[] _drawStepPositionsRight)
        {
            Gizmos.color = Color.green;
            foreach (var point in points)
            {
                Gizmos.DrawWireSphere(point, 0.1f);
            }

            for (int i = 0; i < drawStepPositions.Length - 1; i++)
            {
                int colorH = (int)(i / (float)drawStepPositions.Length * 360);
                Gizmos.color = GetColorByHsv(colorH);
                Gizmos.DrawLine(drawStepPositions[i], drawStepPositions[i + 1]);
            }

            if (_drawStepPositionsLeft != null && _drawStepPositionsRight != null)
            {
                for (int i = 0; i < _drawStepPositionsLeft.Length - 1; i++)
                {
                    int colorH = (int)(i / (float)_drawStepPositionsLeft.Length * 360);
                    Gizmos.color = GetColorByHsv(colorH);
                    Gizmos.DrawLine(_drawStepPositionsLeft[i], _drawStepPositionsLeft[i + 1]);
                    Gizmos.DrawLine(_drawStepPositionsRight[i], _drawStepPositionsRight[i + 1]);
                }
            }
        }

        public static Color32 GetColorByHsv(int h)
        {
            float s = 1, v = 1;
            Color32 c = new Color32();
            int i = (int)(h / 60f);
            float f = h / 60f - i;
            byte p1 = (byte)(v * (1 - s) * 255);
            byte p2 = (byte)(v * (1 - s * f) * 255);
            byte p3 = (byte)(v * (1 - s * (1 - f)) * 255);
            byte vi = (byte)(v * 255);
            byte r = 0, g = 0, b = 0;
            switch (i)
            {
                case 0: r = vi; g = p3; b = p1; break;
                case 1: r = p2; g = vi; b = p1; break;
                case 2: r = p1; g = vi; b = p3; break;
                case 3: r = p1; g = p2; b = vi; break;
                case 4: r = p3; g = p1; b = vi; break;
                case 5: r = vi; g = p1; b = p2; break;
                default: break;
            }
            c.a = 255;
            c.r = r;
            c.g = g;
            c.b = b;
            return c;
        }
    }
}
#endif

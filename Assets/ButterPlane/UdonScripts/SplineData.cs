using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SplineData : UdonSharpBehaviour
    {
        const int B_SPLINE_DIM = 3;
        const int TX_STEP = 20;

        float[][] _a, _b, _c, _d, _w;
        float[] _txTable;
        [SerializeField] int _halfAddPointCount = 2;

        public int GetPointCount()
        {
            if (_a == null || _a.Length == 0)
            {
                return 0;
            }

            return _a[0].Length;
        }

        public void Initialize(ref Vector3[] points, bool isLoop = true)
        {
            _a = null;
            _b = null;
            _c = null;
            _d = null;
            _w = null;
            _txTable = null;

            if (points == null || points.Length <= 1)
            {
                return;
            }

            if (isLoop)
            {
                var additionalPointCount = _halfAddPointCount * 2;
                var loopPoints = new Vector3[points.Length + additionalPointCount + 1];
                for (int i = 0; i < loopPoints.Length; i++)
                {
                    loopPoints[i] = points[(i - _halfAddPointCount + points.Length) % points.Length];
                }
                SplineUtils.InitLoopBSpline(ref _a, ref _b, ref _c, ref _d, ref _w, ref loopPoints, ref _txTable, B_SPLINE_DIM, TX_STEP);

                int len = points.Length;
                int dim = _a.Length;
                float[][] __a, __b, __c, __d, __w;
                __a = new float[dim][];
                __b = new float[dim][];
                __c = new float[dim][];
                __d = new float[dim][];
                __w = new float[dim][];
                for (int i = 0; i < dim; i++)
                {
                    __a[i] = new float[len];
                    __b[i] = new float[len];
                    __c[i] = new float[len];
                    __d[i] = new float[len];
                    __w[i] = new float[len];
                }
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        __a[j][i] = _a[j][i + _halfAddPointCount];
                        __b[j][i] = _b[j][i + _halfAddPointCount];
                        __c[j][i] = _c[j][i + _halfAddPointCount];
                        __d[j][i] = _d[j][i + _halfAddPointCount];
                        __w[j][i] = _w[j][i + _halfAddPointCount];
                    }
                }
                _a = __a;
                _b = __b;
                _c = __c;
                _d = __d;
                _w = __w;
                SplineUtils.InitBSplineTXTable(ref _a, ref _b, ref _c, ref _d, ref _txTable, TX_STEP, true);
            }
            else
            {
                SplineUtils.InitBSpline(ref _a, ref _b, ref _c, ref _d, ref _w, ref points, ref _txTable, B_SPLINE_DIM, TX_STEP);
            }
        }

        public Vector3 CalcPosition(float t)
        {
            return SplineUtils.CalcBSplinePosition(ref _a, ref _b, ref _c, ref _d, t);
        }

        public Vector3 CalcPositionByDistance(float x)
        {
            return SplineUtils.CalcBSplinePosition(ref _a, ref _b, ref _c, ref _d, ref _txTable, x, TX_STEP);
        }
    }

    public static class SplineUtils
    {
        public static void InitBSpline(ref float[][] a, ref float[][] b, ref float[][] c, ref float[][] d, ref float[][] w, ref Vector3[] points, ref float[] txTable, int dim, int txStep)
        {
            InitBSplineArrays(ref a, ref b, ref c, ref d, ref w, points.Length, dim);
            InitBSplineWehghtA(ref points, ref a);
            InitBSplineWehghts(ref a, ref b, ref c, ref d, ref w);
            InitBSplineTXTable(ref a, ref b, ref c, ref d, ref txTable, txStep);
        }

        public static void InitLoopBSpline(ref float[][] a, ref float[][] b, ref float[][] c, ref float[][] d, ref float[][] w, ref Vector3[] points, ref float[] txTable, int dim, int txStep)
        {
            InitBSplineArrays(ref a, ref b, ref c, ref d, ref w, points.Length, dim);
            InitBSplineWehghtA(ref points, ref a);
            InitBSplineWehghts(ref a, ref b, ref c, ref d, ref w);
            InitBSplineTXTable(ref a, ref b, ref c, ref d, ref txTable, txStep);
        }

        static void InitBSplineArrays(ref float[][] a, ref float[][] b, ref float[][] c, ref float[][] d, ref float[][] w, int len, int dim)
        {
            a = new float[dim][];
            b = new float[dim][];
            c = new float[dim][];
            d = new float[dim][];
            w = new float[dim][];
            for (int j = 0; j < dim; j++)
            {
                a[j] = new float[len];
                b[j] = new float[len];
                c[j] = new float[len];
                d[j] = new float[len];
                w[j] = new float[len];
            }
        }

        /// <summary>
        /// 参考: https://www5d.biglobe.ne.jp/~stssk/maze/spline.html
        /// </summary>
        static void InitBSplineWehghtA(ref Vector3[] points, ref float[][] a)
        {
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = 0; j < a.Length; j++)
                {
                    a[j][i] = points[i][j];
                }
            }
        }

        static void InitBSplineWehghts(ref float[][] a, ref float[][] b, ref float[][] c, ref float[][] d, ref float[][] w)
        {
            int bSplineDim = a.Length;
            int len = a[0].Length;

            for (int j = 0; j < bSplineDim; j++)
            {
                c[j][0] = c[j][len - 1] = 0f;
            }
            for (int i = 1; i < len - 1; i++)
            {
                for (int j = 0; j < bSplineDim; j++)
                {
                    c[j][i] = 3.0f * (a[j][i - 1] - 2.0f * a[j][i] + a[j][i + 1]);
                }
            }

            for (int j = 0; j < bSplineDim; j++)
            {
                w[j][0] = 0.0f;
            }
            for (int i = 1; i < len - 1; i++)
            {
                for (int j = 0; j < bSplineDim; j++)
                {
                    float tmp = 4.0f - w[j][i - 1];
                    c[j][i] = (c[j][i] - c[j][i - 1]) / tmp;
                    w[j][i] = 1.0f / tmp;
                }
            }

            for (int i = len - 2; i > 0; i--)
            {
                for (int j = 0; j < bSplineDim; j++)
                {
                    c[j][i] = c[j][i] - c[j][i + 1] * w[j][i];
                }
            }

            for (int j = 0; j < bSplineDim; j++)
            {
                b[j][len - 1] = d[j][len - 1] = 0.0f;
            }
            for (int i = 0; i < len - 1; i++)
            {
                for (int j = 0; j < bSplineDim; j++)
                {
                    d[j][i] = (c[j][i + 1] - c[j][i]) / 3.0f;
                    b[j][i] = a[j][i + 1] - a[j][i] - c[j][i] - d[j][i];
                }
            }
        }

        public static void InitBSplineTXTable(ref float[][] a, ref float[][] b, ref float[][] c, ref float[][] d, ref float[] txTable, int txStep, bool isLoop = false)
        {
            int len = a[0].Length;
            if (isLoop)
            {
                len++;
            }
            txTable = new float[txStep * (len - 1) + 1];

            int j = 0;
            Vector3 prePos = CalcBSplinePosition(ref a, ref b, ref c, ref d, 0);
            float distance = 0;
            for (int s = 0; s < len - 1; s++)
            {
                for (int step = 0; step < txStep; step++)
                {
                    float t = s + (float)step / txStep;
                    var pos = CalcBSplinePosition(ref a, ref b, ref c, ref d, t);
                    distance += (pos - prePos).magnitude;
                    txTable[j++] = distance;
                    prePos = pos;
                }
            }
            {
                var pos = CalcBSplinePosition(ref a, ref b, ref c, ref d, len - 1);
                distance += (pos - prePos).magnitude;
                txTable[j] = distance;
            }
        }

        public static Vector3 CalcBSplinePosition(ref float[][] a, ref float[][] b, ref float[][] c, ref float[][] d, float t)
        {
            int bSplineDim = a.Length;
            t %= a[0].Length;
            if (t < 0)
            {
                t = 0;
            }

            int i = (int)t;
            float dt = t - i;
            var res = Vector3.zero;
            for (int j = 0; j < bSplineDim; j++)
            {
                res[j] = a[j][i] + (b[j][i] + (c[j][i] + d[j][i] * dt) * dt) * dt;
            }
            return res;
        }

        public static Vector3 CalcBSplinePosition(ref float[][] a, ref float[][] b, ref float[][] c, ref float[][] d, ref float[] txTable, float x, int txStep)
        {
            var t = CalcT(ref txTable, x, txStep);
            return CalcBSplinePosition(ref a, ref b, ref c, ref d, t);
        }

        static float CalcT(ref float[] txTable, float x, int txStep)
        {
            x %= txTable[txTable.Length - 1];
            for (int i = txTable.Length - 1; i >= 0; i--)
            {
                var l = txTable[i];
                if (l > x)
                {
                    continue;
                }
                var diff = x - l;
                var sectionDistance = txTable[i + 1] - l;
                var r = diff / sectionDistance;
                return (i + r) / txStep;
            }
            return 0;
        }
    }
}

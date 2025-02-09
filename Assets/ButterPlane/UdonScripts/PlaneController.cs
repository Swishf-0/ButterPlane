using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlaneController : UdonSharpBehaviour
    {
        [SerializeField] Plane[] _planes;
        [SerializeField] float _offset = 0.5f;
        [SerializeField] float _speed = 1f;
        [SerializeField] Course _course;

        float _x, _preX;
        Vector3 _prePos;

        public void Initialize()
        {
            foreach (var plane in _planes)
            {
                plane.Initialize();
            }

            _course.Initialize();

            _preX = 0;
        }

        public void Update_()
        {
            MovePlane();

            foreach (var plane in _planes)
            {
                plane.Update_();
            }
        }

        void MovePlane()
        {
            _x += _speed * Time.deltaTime;
            var pos = _course.GetPositionByDistance(_x);
            if (_preX == 0)
            {
                _preX = 0;
                _prePos = _course.GetPositionByDistance(_preX);
            }

            var nextX = _x += _speed * Time.deltaTime;
            var nextPos = _course.GetPositionByDistance(nextX);

            var dirNextPre = nextPos - _prePos;
            var dirOffsetRight = -Vector3.Cross(dirNextPre, Vector3.up).normalized;

            var dir = pos - _prePos;
            float offset = (_planes.Length - 1) / 2f;
            for (int i = 0; i < _planes.Length; i++)
            {
                _planes[i].transform.SetPositionAndRotation(pos - (offset - i) * _offset * dirOffsetRight, Quaternion.LookRotation(dir, Vector3.up));
            }

            _preX = _x;
            _prePos = pos;
        }
    }
}

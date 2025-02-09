using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Course : UdonSharpBehaviour
    {
        [SerializeField] Transform _courceAnchorRoot;

        SplineData _splineData;

        public void Initialize()
        {
            _splineData = GetComponent<SplineData>();
            var points = MiscUtils.GetChildrenPositions(_courceAnchorRoot, Vector3.zero);
            _splineData.Initialize(ref points);
        }

        public Vector3 GetPosition(float t)
        {
            return _splineData.CalcPosition(t);
        }

        public Vector3 GetPositionByDistance(float x)
        {
            return _splineData.CalcPositionByDistance(x);
        }
    }
}

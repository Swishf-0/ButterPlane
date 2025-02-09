using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlaneEffectColliderSensor : UdonSharpBehaviour
    {
        PlaneEffectController _controller;
        Transform _colliderRoot;

        public void Initialize(PlaneEffectController controller, Transform colliderRoot)
        {
            _controller = controller;
            _colliderRoot = colliderRoot;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent != _colliderRoot)
            {
                return;
            }

            _controller.PlayEffect(other.GetComponent<PlaneEffectInfo>());
        }
    }
}

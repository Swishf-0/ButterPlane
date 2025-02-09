using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlaneEffectInfo : UdonSharpBehaviour
    {
        public PlaneEffectType EffectType { get => _effectType; set => _effectType = value; }
        public string Letters { get => _letters; set => _letters = value; }
        public float Duration { get => _duration; set => _duration = value; }

        [SerializeField] PlaneEffectType _effectType;
        [SerializeField] string _letters;
        [SerializeField] float _duration;
    }
}

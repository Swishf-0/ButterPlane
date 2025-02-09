using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    public enum PlaneEffectType
    {
        DefaultSmoke = 0,
        RainbowSmoke = 1,
        LetterEffect = 2,
        Custom_1 = 3,
        Custom_2 = 4,
        Custom_3 = 5,
        __MAX__,
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlaneEffect : UdonSharpBehaviour
    {
        [SerializeField] ParticleSystem[] _effects;

        ParticleSystem _currentEffect;

        public void Initialize()
        {
            StopEffects();
        }

        public void Update_()
        {

        }

        public void PlayeEffect()
        {
            if (_currentEffect != null)
            {
                _currentEffect.Play();
            }
        }

        public void StopEffect()
        {
            StopEffects();
        }

        public void SwitchEffect(PlaneEffectType effectType)
        {
            SwitchEffect(GetEffect(effectType));
        }

        ParticleSystem GetEffect(PlaneEffectType effectType)
        {
            var effectTypeIdx = (int)effectType;
            if (effectTypeIdx < 0 || _effects.Length <= effectTypeIdx)
            {
                return null;
            }

            return _effects[effectTypeIdx];
        }

        void SwitchEffect(ParticleSystem particle)
        {
            if (particle == null)
            {
                return;
            }

            if (_currentEffect == particle)
            {
                return;
            }

            if (_currentEffect == null)
            {
                _currentEffect = particle;
                return;
            }

            var playing = _currentEffect.isPlaying;
            StopEffects();
            if (playing)
            {
                particle.Play();
            }
            _currentEffect = particle;
        }

        void StopEffects()
        {
            foreach (var effect in _effects)
            {
                effect.Stop();
            }
        }
    }
}

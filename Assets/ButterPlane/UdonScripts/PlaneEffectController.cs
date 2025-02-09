using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    enum PlaneEffectControllerState
    {
        Idle,
        Playing,
    }

    enum PlaneEffectControllerEffectState
    {
        None,
        WaitTime,
        LetterEffect,
        Smoke,
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlaneEffectController : UdonSharpBehaviour
    {
        [SerializeField] PlaneEffect[] _planeEffects;
        [SerializeField] LetterEffectController _letterEffectController;
        [SerializeField] PlaneEffectColliderSensor _effectSensor;
        [SerializeField] Transform _planeEffectColliderRoot;

        [SerializeField] bool _randomLetterMode;
        [SerializeField] bool _randomSmokeMode;
        [SerializeField] int _randomSmokeDurationMin;
        [SerializeField] int _randomSmokeDurationMax;
        [SerializeField] int _randomIntervalMin;
        [SerializeField] int _randomIntervalMax;
        [SerializeField] string[] _randomLetters;
        [SerializeField] int _randomWeightLetter;
        [SerializeField] int _randomWeightSmoke;
        [SerializeField] PlaneEffectInfo _actionEffectInfo;

        PlaneEffectControllerState _state;
        PlaneEffectControllerEffectState _effectState;
        float _timer;

        public void Initialize()
        {
            _state = PlaneEffectControllerState.Idle;
            _effectState = PlaneEffectControllerEffectState.None;

            foreach (var effect in _planeEffects)
            {
                effect.Initialize();
            }

            _letterEffectController.Initialize(_planeEffects);
            _effectSensor.Initialize(this, _planeEffectColliderRoot);

            if (_randomIntervalMax < _randomIntervalMin)
            {
                _randomIntervalMax = _randomIntervalMin;
            }

            SetNextState();
        }

        public void Update_()
        {
            switch (_state)
            {
                case PlaneEffectControllerState.Idle:
                    {
                        return;
                    }
                case PlaneEffectControllerState.Playing:
                    {
                        switch (_effectState)
                        {
                            case PlaneEffectControllerEffectState.WaitTime:
                                {
                                    if (_timer > Time.time)
                                    {
                                        return;
                                    }

                                    SetNextState();
                                    return;
                                }
                            case PlaneEffectControllerEffectState.Smoke:
                                {
                                    if (_timer > Time.time)
                                    {
                                        return;
                                    }

                                    StopEffect();

                                    SetNextState();
                                    return;
                                }
                            case PlaneEffectControllerEffectState.LetterEffect:
                                {
                                    if (_letterEffectController.IsPlaying)
                                    {
                                        _letterEffectController.Update_();
                                        return;
                                    }

                                    SetNextState();
                                    return;
                                }
                        }
                        return;
                    }
            }
        }

        void SetNextState()
        {
            if (_effectState == PlaneEffectControllerEffectState.WaitTime)
            {
                PlayEffect(_actionEffectInfo);
                return;
            }

            if (!_randomLetterMode && !_randomSmokeMode)
            {
                _state = PlaneEffectControllerState.Idle;
                _effectState = PlaneEffectControllerEffectState.None;
                return;
            }

            _state = PlaneEffectControllerState.Playing;
            _effectState = PlaneEffectControllerEffectState.WaitTime;
            bool isLetterMode = false;
            if (_randomLetterMode && _randomSmokeMode)
            {
                var r = Random.Range(0, _randomWeightLetter + _randomWeightSmoke);
                isLetterMode = r < _randomWeightLetter;
            }
            else
            {
                isLetterMode = _randomLetterMode;
            }


            if (isLetterMode)
            {
                _actionEffectInfo.EffectType = PlaneEffectType.LetterEffect;
                _actionEffectInfo.Letters = _randomLetters[Random.Range(0, _randomLetters.Length)];
            }
            else
            {
                _actionEffectInfo.EffectType = GetRandomSmokeEffect();
                _actionEffectInfo.Duration = Random.Range(_randomSmokeDurationMin, _randomSmokeDurationMax);
            }

            _timer = Time.time + Random.Range(_randomIntervalMin, _randomIntervalMax);
        }

        PlaneEffectType GetRandomSmokeEffect()
        {
            var idx = Random.Range(0, (int)PlaneEffectType.__MAX__ - 1);
            if (idx == (int)PlaneEffectType.RainbowSmoke)
            {
                idx++;
            }
            return (PlaneEffectType)idx;
        }

        public void PlayEffect(PlaneEffectInfo effectInfo)
        {
            switch (effectInfo.EffectType)
            {
                case PlaneEffectType.DefaultSmoke:
                case PlaneEffectType.RainbowSmoke:
                case PlaneEffectType.Custom_1:
                case PlaneEffectType.Custom_2:
                case PlaneEffectType.Custom_3:
                    {
                        _state = PlaneEffectControllerState.Playing;
                        _effectState = PlaneEffectControllerEffectState.Smoke;
                        _timer = Time.time + effectInfo.Duration;
                        SwitchEffect(effectInfo.EffectType);
                        PlayEffect();
                        return;
                    }
                case PlaneEffectType.LetterEffect:
                    {
                        _state = PlaneEffectControllerState.Playing;
                        _effectState = PlaneEffectControllerEffectState.LetterEffect;
                        SwitchEffect(effectInfo.EffectType);
                        _letterEffectController.StartEffect(effectInfo.Letters);
                        return;
                    }
            }

            _effectState = PlaneEffectControllerEffectState.None;
        }

        void SwitchEffect(PlaneEffectType effectType)
        {
            foreach (var effect in _planeEffects)
            {
                effect.SwitchEffect(effectType);
            }
        }

        public void PlayEffect()
        {
            foreach (var effect in _planeEffects)
            {
                effect.PlayeEffect();
            }
        }

        public void StopEffect()
        {
            foreach (var effect in _planeEffects)
            {
                effect.StopEffect();
            }
        }
    }
}

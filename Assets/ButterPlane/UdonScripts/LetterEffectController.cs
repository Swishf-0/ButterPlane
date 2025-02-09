using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    enum LetterEffectControllerState
    {
        WAIT,
        PLAYING,
        INTERVAL,
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class LetterEffectController : UdonSharpBehaviour
    {
        public bool IsPlaying => _state != LetterEffectControllerState.WAIT;

        [SerializeField] bool _reverse;
        [SerializeField] LetterDataSet _letterDataSet;
        [SerializeField] LetterEffect _letterEffect;
        [SerializeField] float _interval = 0.5f;
        [SerializeField] float _timeScale = 1;
        [SerializeField] float _spaceInterval = 0.5f;

        PlaneEffect[] _planeEffects;
        string[] _letterList;
        LetterEffectControllerState _state;
        float _timer;
        int _currentLetterIdx;

        public void Initialize(PlaneEffect[] planeEffects)
        {
            _planeEffects = planeEffects;

            _state = LetterEffectControllerState.WAIT;

            _letterDataSet.Initialize();
            _letterEffect.Initialize();
        }

        public void Update_()
        {
            _letterEffect.Update_();

            switch (_state)
            {
                case LetterEffectControllerState.PLAYING:
                    {
                        if (!_letterEffect.IsPlaying)
                        {
                            _currentLetterIdx++;
                            if (_currentLetterIdx >= _letterList.Length)
                            {
                                _state = LetterEffectControllerState.WAIT;
                                return;
                            }

                            _state = LetterEffectControllerState.INTERVAL;
                            _timer = Time.time + _interval;
                        }
                        return;
                    }
                case LetterEffectControllerState.INTERVAL:
                    {
                        if (Time.time >= _timer)
                        {
                            _state = LetterEffectControllerState.PLAYING;

                            if (_currentLetterIdx >= _letterList.Length)
                            {
                                Debug.Log($"_currentLetterIdx >= _letterList.Length: {_currentLetterIdx} >= {_letterList.Length}");
                                _state = LetterEffectControllerState.WAIT;
                                return;
                            }
                            var letter = _letterList[_currentLetterIdx];
                            if (letter == " ")
                            {
                                _currentLetterIdx++;
                                if (_currentLetterIdx >= _letterList.Length)
                                {
                                    _state = LetterEffectControllerState.WAIT;
                                    return;
                                }

                                _state = LetterEffectControllerState.INTERVAL;
                                _timer = Time.time + _spaceInterval;
                                return;
                            }
                            _letterDataSet.GetLetterData(letter, out var letterDataStartList, out var letterDataEndList);
                            _letterEffect.StartEffect(_planeEffects, letterDataStartList, letterDataEndList, _timeScale, _reverse);
                        }
                        return;
                    }
            }
        }

        public void StartEffect(string letters)
        {
            _letterList = MiscUtils.Split(letters);

            _currentLetterIdx = 0;
            _timer = 0;
            _state = LetterEffectControllerState.INTERVAL;
        }
    }
}

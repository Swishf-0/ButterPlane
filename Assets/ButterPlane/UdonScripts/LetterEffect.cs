using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    enum LetterEffectState
    {
        WAIT,
        PLAYING,
    }

    enum LineState
    {
        WAIT,
        WAIT_START,
        WAIT_END,
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class LetterEffect : UdonSharpBehaviour
    {
        public bool IsPlaying => _playState != LetterEffectState.WAIT;

        PlaneEffect[] _planeEffects;
        float[][] _letterDataStartList;
        float[][] _letterDataEndList;
        float _timeScale;

        LineState[] _lineStates;
        LetterEffectState _playState;
        float _startTime;
        float[] _timers;
        int[] _idxList;
        bool[] _endFlags;

        public void Initialize()
        {
            _playState = LetterEffectState.WAIT;
        }

        public void StartEffect(PlaneEffect[] planeEffects, float[][] letterDataStartList, float[][] letterDataEndList, float timeScale, bool reverse)
        {
            if (planeEffects.Length != letterDataStartList.Length)
            {
                return;
            }

            if (reverse)
            {
                _planeEffects = new PlaneEffect[planeEffects.Length];
                for (int i = 0; i < _planeEffects.Length; i++)
                {
                    _planeEffects[i] = planeEffects[_planeEffects.Length - i - 1];
                }
            }
            else
            {
                _planeEffects = planeEffects;
            }
            _letterDataStartList = letterDataStartList;
            _letterDataEndList = letterDataEndList;
            _timeScale = timeScale;

            _timers = new float[_planeEffects.Length];
            _idxList = new int[_planeEffects.Length];
            _lineStates = new LineState[_planeEffects.Length];
            _endFlags = new bool[_planeEffects.Length];

            _startTime = Time.time;
            for (int i = 0; i < _planeEffects.Length; i++)
            {
                _idxList[i] = 0;
                UpdateStartData(i);
            }

            _playState = LetterEffectState.PLAYING;
        }

        void EffectOn(int i)
        {
            _planeEffects[i].PlayeEffect();
        }

        void EffectOff(int i)
        {
            _planeEffects[i].StopEffect();
        }

        bool UpdateEndFlag(int i)
        {
            _endFlags[i] = _idxList[i] >= _letterDataStartList[i].Length;
            if (_endFlags[i])
            {
                _lineStates[i] = LineState.WAIT;
            }
            return _endFlags[i];
        }

        void UpdateStartData(int i)
        {
            if (UpdateEndFlag(i))
            {
                return;
            }

            _lineStates[i] = LineState.WAIT_START;
            _timers[i] = _startTime + _letterDataStartList[i][_idxList[i]] * _timeScale;
        }

        void UpdateEndData(int i)
        {
            if (UpdateEndFlag(i))
            {
                return;
            }

            _lineStates[i] = LineState.WAIT_END;
            _timers[i] = _startTime + _letterDataEndList[i][_idxList[i]] * _timeScale;
        }

        public void Update_()
        {
            switch (_playState)
            {
                case LetterEffectState.WAIT:
                    {
                        return;
                    }
                case LetterEffectState.PLAYING:
                    {
                        UpdateEffects();
                        return;
                    }
            }
        }

        public void UpdateEffects()
        {
            for (int i = 0; i < _letterDataStartList.Length; i++)
            {
                if (_endFlags[i])
                {
                    continue;
                }

                if (Time.time < _timers[i])
                {
                    continue;
                }

                switch (_lineStates[i])
                {
                    case LineState.WAIT_START:
                        {
                            UpdateEndData(i);

                            if (!_endFlags[i])
                            {
                                EffectOn(i);
                            }
                            else
                            {
                                if (MiscUtils.All(_endFlags))
                                {
                                    _playState = LetterEffectState.WAIT;
                                    return;
                                }
                            }
                            return;
                        }
                    case LineState.WAIT_END:
                        {
                            _idxList[i]++;
                            UpdateStartData(i);
                            EffectOff(i);

                            if (_endFlags[i])
                            {
                                if (MiscUtils.All(_endFlags))
                                {
                                    _playState = LetterEffectState.WAIT;
                                    return;
                                }
                            }
                            return;
                        }
                }
            }
        }
    }
}

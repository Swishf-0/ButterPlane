using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Plane : UdonSharpBehaviour
    {
        [SerializeField] Transform _root_0;
        [SerializeField] Transform _root_1;

        [SerializeField] bool _useRollAction = true;
        [SerializeField] float _nextShakeTimeMin = 5;
        [SerializeField] float _nextShakeTimeMax = 20;
        [SerializeField] float _shakePowerMin = 50;
        [SerializeField] float _shakePowerMax = 300;
        [SerializeField] float _shakePowerDecayRate = 1;
        [SerializeField] float _curveRollRate = 30;

        float _rotateV;
        float _rotateA;
        float _angleZ;
        float _timer;
        float _targetAngle;

        Vector3 _prePos, _preDir;

        public void Initialize()
        {
            _angleZ = 0;
            _timer = Time.time + Random.Range(_nextShakeTimeMin, _nextShakeTimeMax);

            PID_Initialize();
        }

        public void Update_()
        {
            if (!_useRollAction)
            {
                return;
            }

            {
                _rotateV += _rotateA * Time.deltaTime;
                _angleZ += _rotateV * Time.deltaTime;
                _angleZ = NormalizeAngle(_angleZ);
                _root_1.localEulerAngles = new Vector3(0, 0, _angleZ);
            }

            {
                _rotateA -= _rotateA * _shakePowerDecayRate * Time.deltaTime;
                if (_timer < Time.time)
                {
                    _timer = Time.time + Random.Range(_nextShakeTimeMin, _nextShakeTimeMax);
                    _rotateA += Random.Range(_shakePowerMin, _shakePowerMax) * Mathf.Sign(Random.Range(-1f, 1f));
                }

                RestorePosition(_targetAngle);

                var dir = (transform.position - _prePos).normalized;
                dir.y = 0;
                if (IsZero(dir) || IsZero(_preDir))
                {
                    _targetAngle = 0;
                }
                else
                {
                    _targetAngle = NormalizeAngle(-Vector3.SignedAngle(_preDir, dir, Vector3.up) * _curveRollRate);
                    _targetAngle = Mathf.Clamp(_targetAngle, -90, 90);
                }
                _prePos = transform.position;
                _preDir = dir;
            }
        }

        bool IsZero(Vector3 v)
        {
            return Mathf.Approximately(v.x, 0) && Mathf.Approximately(v.y, 0) && Mathf.Approximately(v.z, 0);
        }

        void RestorePosition(float targetAngle)
        {
            float error = NormalizeAngle(targetAngle) - NormalizeAngle(_angleZ);
            error = NormalizeAngle(error);

            PID_CalcStep(error);
            _rotateV += _pid_pidValue * Time.deltaTime;
        }

        float NormalizeAngle(float angle)
        {
            angle %= 360;
            if (angle > 180)
            {
                angle -= 360;
            }
            else if (angle < -180)
            {
                angle += 360;
            }
            return angle;
        }

        [SerializeField] float _pid_Kp = 5;
        [SerializeField] float _pid_Ki = 0.1f;
        [SerializeField] float _pid_Kd = 3;

        float _pid_error;
        float _pid_lastError, _pid_p, _pid_i, _pid_d;
        float _pid_pidValue;

        void PID_Initialize()
        {
            _pid_lastError = 0;
        }

        void PID_CalcStep(float error)
        {
            _pid_error = error;
            _pid_p = _pid_error;
            _pid_i += (_pid_error + _pid_lastError) * Time.deltaTime * 0.5f;
            _pid_d = (_pid_error - _pid_lastError) / Time.deltaTime;
            _pid_lastError = _pid_error;
            _pid_pidValue = _pid_p * _pid_Kp + _pid_i * _pid_Ki + _pid_d * _pid_Kd;
        }
    }
}

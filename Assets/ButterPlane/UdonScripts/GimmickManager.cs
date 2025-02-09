using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class GimmickManager : UdonSharpBehaviour
    {
        [SerializeField] PlaneController _planeController;
        [SerializeField] PlaneEffectController _planeEffectController;

        bool _initialized = false;

        void Start() { Initialize(); }
        void Update() { Update_(); }

        public void Initialize()
        {
            _planeController.Initialize();
            _planeEffectController.Initialize();

            _initialized = true;
        }

        void Update_()
        {
            if (!_initialized)
            {
                return;
            }

            _planeController.Update_();
            _planeEffectController.Update_();
        }
    }

    public static class MiscUtils
    {
        public static Vector3[] GetChildrenPositions(Transform root, Vector3 offset)
        {
            var positions = new Vector3[root.childCount];
            for (int i = 0; i < root.childCount; i++)
            {
                positions[i] = root.GetChild(i).position - offset;
            }
            return positions;
        }

        public static bool All(bool[] arr)
        {
            foreach (var value in arr)
            {
                if (!value)
                {
                    return false;
                }
            }

            return true;
        }

        public static void ToIntArray(float[] value, out int[] res, int digit)
        {
            if (value == null)
            {
                res = null;
                return;
            }

            res = new int[value.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = Mathf.RoundToInt(value[i] * digit);
            }
        }

        public static void ToFloatArray(int[] value, out float[] res, int digit)
        {
            if (value == null)
            {
                res = null;
                return;
            }

            float invDigit = 1 / (float)digit;
            res = new float[value.Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = value[i] * invDigit;
            }
        }

        public static void ToIntArray(string value, out int[] res, string separator = ",")
        {
            var stringArray = value.Split(separator);
            res = new int[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (int.TryParse(stringArray[i], out var v))
                {
                    res[i] = v;
                }
            }
        }

        public static string ToString(int[] value, string separator = ",")
        {
            // return System.String.Join(separator, value);
            string res = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (!string.IsNullOrEmpty(res))
                {
                    res += separator;
                }
                res += value[i].ToString();
            }
            return res;
        }

        public static string[] Split(string letters)
        {
            var chars = letters.ToCharArray();
            var letterList = new string[chars.Length];
            for (int i = 0; i < letterList.Length; i++)
            {
                letterList[i] = chars[i].ToString();
            }
            return letterList;
        }

        public static void SetPositionX(Transform t, float x)
        {
            t.position = new Vector3(x, t.position.y, t.position.z);
        }

        public static void SetPositionY(Transform t, float y)
        {
            t.position = new Vector3(t.position.x, y, t.position.z);
        }

        public static void SetLocalPositionY(Transform t, float y)
        {
            t.localPosition = new Vector3(t.localPosition.x, y, t.localPosition.z);
        }

        public static void SetLocalPositionZ(Transform t, float z)
        {
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, z);
        }
    }
}

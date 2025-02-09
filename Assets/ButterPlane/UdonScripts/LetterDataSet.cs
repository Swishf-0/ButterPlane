using System;
using UdonSharp;
using UnityEngine;

namespace Swishf.ButterPlane
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class LetterDataSet : UdonSharpBehaviour
    {
        public const int DIGIT = 1000;

        public string[] LetterDataTextList { get => _letterDataTextList; set => _letterDataTextList = value; }
        public string[] LetterDataNames { get => _letterDataNames; set => _letterDataNames = value; }
        public float[][][] LetterDataSetStartList => _letterDataSetStartList;
        public float[][][] LetterDataSetEndList => _letterDataSetEndList;

        [SerializeField, TextArea(1, 5)] string[] _letterDataTextList;
        [SerializeField] string[] _letterDataNames;

        float[][][] _letterDataSetStartList;
        float[][][] _letterDataSetEndList;

        public void Initialize()
        {
            LetterDataUtils.TextToData(_letterDataTextList, out _letterDataSetStartList, out _letterDataSetEndList, DIGIT);
        }

        public void GetLetterData(string letterDataName, out float[][] letterDataStartList, out float[][] letterDataEndList)
        {
            var idx = Array.IndexOf(_letterDataNames, letterDataName);
            if (idx < 0)
            {
                letterDataStartList = null;
                letterDataEndList = null;
            }
            letterDataStartList = _letterDataSetStartList[idx];
            letterDataEndList = _letterDataSetEndList[idx];
        }
    }

    public static class LetterDataUtils
    {
        public static bool IsValid(LetterDataSet data)
        {
            return IsValid(data.LetterDataSetStartList, data.LetterDataSetEndList);
        }

        public static bool IsValid(float[][][] letterDataSetStartList, float[][][] letterDataSetEndList)
        {
            if (letterDataSetStartList == null && letterDataSetEndList == null)
            {
                return true;
            }

            if (letterDataSetStartList == null || letterDataSetEndList == null)
            {
                return false;
            }

            return letterDataSetStartList.Length == letterDataSetEndList.Length;
        }

        public static bool IsEpmty(LetterDataSet data)
        {
            return IsEpmty(data.LetterDataSetStartList, data.LetterDataSetEndList);
        }

        public static bool IsEpmty(float[][][] letterDataSetStartList, float[][][] letterDataSetEndList)
        {
            if (!IsValid(letterDataSetStartList, letterDataSetEndList))
            {
                return true;
            }

            return letterDataSetStartList == null || letterDataSetStartList.Length == 0;
        }

        public static bool TextToData(string[] letterDataTextList, out float[][][] letterDataSetStartList, out float[][][] letterDataSetEndList, int digit)
        {
            letterDataSetStartList = null;
            letterDataSetEndList = null;

            if (IsEmpty(letterDataTextList))
            {
                return false;
            }

            letterDataSetStartList = new float[letterDataTextList.Length][][];
            letterDataSetEndList = new float[letterDataTextList.Length][][];
            for (int i = 0; i < letterDataTextList.Length; i++)
            {
                TextToData(letterDataTextList[i], out letterDataSetStartList[i], out letterDataSetEndList[i], digit);
            }
            return true;
        }

        static void TextToData(string letterDataText, out float[][] letterDataStartList, out float[][] letterDataEndList, int digit)
        {
            var textDataList = letterDataText.Split("\n");
            letterDataStartList = new float[textDataList.Length][];
            letterDataEndList = new float[textDataList.Length][];
            for (int i = 0; i < textDataList.Length; i++)
            {
                TextToData(textDataList[i], out letterDataStartList[i], out letterDataEndList[i], digit);
            }
        }

        static void TextToData(string lineText, out float[] lineStartPoints, out float[] lineEndPoints, int digit)
        {
            var textDataList = lineText.Split("@");
            MiscUtils.ToIntArray(textDataList[0], out var dataStartInt);
            MiscUtils.ToIntArray(textDataList[1], out var dataEndInt);
            MiscUtils.ToFloatArray(dataStartInt, out lineStartPoints, digit);
            MiscUtils.ToFloatArray(dataEndInt, out lineEndPoints, digit);
        }

        static bool IsEmpty(string[] letterDataTextList)
        {
            if (letterDataTextList.Length == 0)
            {
                return true;
            }

            foreach (var text in letterDataTextList)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool LetterDataSetToText(float[][][] letterDataSetStartList, float[][][] letterDataSetEndList, out string[] letterDataTextList, int digit)
        {
            if (IsEpmty(letterDataSetStartList, letterDataSetEndList))
            {
                letterDataTextList = null;
                return false;
            }

            letterDataTextList = new string[letterDataSetStartList.Length];

            for (int i = 0; i < letterDataSetStartList.Length; i++)
            {
                letterDataTextList[i] = LetterDataToText(letterDataSetStartList[i], letterDataSetEndList[i], digit); ;
            }

            return true;
        }

        public static string LetterDataToText(float[][] letterDataStartList, float[][] letterDataEndList, int digit)
        {

            var text = "";
            for (int j = 0; j < letterDataStartList.Length; j++)
            {
                MiscUtils.ToIntArray(letterDataStartList[j], out int[] dataStartInt, digit);
                MiscUtils.ToIntArray(letterDataEndList[j], out int[] dataEndtInt, digit);
                if (!string.IsNullOrEmpty(text))
                {
                    text += "\n";
                }
                text += $"{MiscUtils.ToString(dataStartInt)}@{MiscUtils.ToString(dataEndtInt)}";
            }
            return text;
        }
    }
}

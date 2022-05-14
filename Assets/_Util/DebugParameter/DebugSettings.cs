using UnityEngine;
using System;

namespace _Util
{
    public partial class DebugSettings : ScriptableObject
    {
        [Serializable]
        public class SampleParameter
        {
            [Header("編集できるストリング型")]
            public String SampleString = "HogeHoge";

            [Header("編集禁止のストリング型"),NonEditable]
            public String SampleNotEditString = "HogeHoge";
        }

        public SampleParameter Sample;
    }
}

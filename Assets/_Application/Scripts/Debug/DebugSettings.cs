using UnityEngine;
using System;
using _Application;

namespace _Util
{
    public partial class DebugSettings : ScriptableObject
    {
        [Serializable]
        public class DebugParameter
        {
            [Header("初回起動のシーンを変更する")]
            public StageId UseDebugSkipScene = StageId.None;
        }

        public DebugParameter Debug;
    }
}
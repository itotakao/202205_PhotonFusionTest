using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Util
{
    public class DebugParameter : MonoBehaviour
    {
        public static DebugSettings Data;

        [SerializeField]
        private DebugSettings _loadData;
        public DebugSettings LoadData
        {
            get { return _loadData; }
            private set { _loadData = value; }
        }

        private void Awake()
        {
            Data = LoadData;
        }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using _Application;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Util
{
    public class AssetsFacade : MonoBehaviour
    {
        public static AssetsFacade Current { get; private set; }

        [SerializeField]
        private UnityEvent OnSetup = new UnityEvent();
        [SerializeField]
        private UnityEvent OnStart = new UnityEvent();
        [SerializeField]
        private UnityEvent OnUpdate = new UnityEvent();
        [SerializeField]
        private UnityEvent OnFixedUpdate = new UnityEvent();
        [SerializeField]
        private UnityEvent OnLateUpdate = new UnityEvent();
        [SerializeField]
        private UnityEvent OnExit = new UnityEvent();

        [System.Serializable]
        private class EventClass
        {
            public string Name;
            public UnityEvent Event;
        }
        [SerializeField]
        private EventClass[] utilityEvents;

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetInspectorUI();
        }

        public void SetInspectorUI()
        {
            OnSetup = new UnityEvent();
            foreach (IAssetsFacadeSetup i in FindObjectOfInterfaces<IAssetsFacadeSetup>())
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(OnSetup, i.OnSetup);
            }

            OnStart = new UnityEvent();
            foreach (IAssetsFacadeStart i in FindObjectOfInterfaces<IAssetsFacadeStart>())
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(OnStart, i.OnStart);
            }

            OnUpdate = new UnityEvent();
            foreach (IAssetsFacadeUpdate i in FindObjectOfInterfaces<IAssetsFacadeUpdate>())
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(OnUpdate, i.OnUpdate);
            }

            OnFixedUpdate = new UnityEvent();
            foreach (IAssetsFacadeFixedUpdate i in FindObjectOfInterfaces<IAssetsFacadeFixedUpdate>())
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(OnFixedUpdate, i.OnFixedUpdate);
            }

            OnLateUpdate = new UnityEvent();
            foreach (IAssetsFacadeLateUpdate i in FindObjectOfInterfaces<IAssetsFacadeLateUpdate>())
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(OnLateUpdate, i.OnLateUpdate);
            }

            OnExit = new UnityEvent();
            foreach (IAssetsFacadeExit i in FindObjectOfInterfaces<IAssetsFacadeExit>())
            {
                UnityEditor.Events.UnityEventTools.AddPersistentListener(OnExit, i.OnExit);
            }

            // UtilityEvent
            List<string> eventNames = utilityEvents.Select(x => x.Name).ToList();
            for (int i = 0; i < eventNames.Count; i++)
            {
                utilityEvents[i] = new EventClass();
                utilityEvents[i].Name = eventNames[i];
                utilityEvents[i].Event = new UnityEvent();
            }

            // UtilityEvent を追加したい場合は下記のように行う
            // UtilityEvent : Home
            //UnityEvent onIdle = SeacrchUtilityEvnet("OnIdle");
            //UnityEvent onAbout = SeacrchUtilityEvnet("OnAbout");
            //UnityEvent onSetting = SeacrchUtilityEvnet("OnSetting");
            //UnityEvent onStart = SeacrchUtilityEvnet("OnStart");
            //UnityEvent onPause = SeacrchUtilityEvnet("OnPause");
            //UnityEvent onRestart = SeacrchUtilityEvnet("OnRestart");
            //UnityEvent onFinish = SeacrchUtilityEvnet("OnFinish");
            //foreach (IAssetsFacadeHomeUtilityEvnets i in FindObjectOfInterfaces<IAssetsFacadeHomeUtilityEvnets>())
            //{
            //    UnityEditor.Events.UnityEventTools.AddPersistentListener(onIdle, i.OnIdle);
            //    UnityEditor.Events.UnityEventTools.AddPersistentListener(onAbout, i.OnAbout);
            //    UnityEditor.Events.UnityEventTools.AddPersistentListener(onSetting, i.OnSetting);
            //    UnityEditor.Events.UnityEventTools.AddPersistentListener(onStart, i.OnStart);
            //    UnityEditor.Events.UnityEventTools.AddPersistentListener(onPause, i.OnPause);
            //    UnityEditor.Events.UnityEventTools.AddPersistentListener(onRestart, i.OnRestart);
            //    UnityEditor.Events.UnityEventTools.AddPersistentListener(onFinish, i.OnFinish);
            //}

            //UnityEvent onTimerUpdate = SeacrchUtilityEvnet("OnTimerUpdate");
            //foreach (IAssetsFacadeTimerUpdate i in FindObjectOfInterfaces<IAssetsFacadeTimerUpdate>())
            //{
            //    UnityEditor.Events.UnityEventTools.AddPersistentListener(onTimerUpdate, i.OnTimerUpdate);
            //}
        }

        private UnityEvent SeacrchUtilityEvnet(string eventName)
        {
            foreach (var e in utilityEvents)
            {
                if (e.Name == eventName)
                {
                    return e.Event;
                }
            }

            return null;
        }
#endif

        private void Awake()
        {
            Current = this;
        }

        private void OnDestroy()
        {
            Current = (this);
        }

        public void InvokeUtilityEvnet(string eventName)
        {
            EventClass[] tmpArray = utilityEvents.Where(o => o.Name.IndexOf(eventName) >= 0).ToArray();

            if (tmpArray.Length == 0)
            {
                throw new ArgumentException(string.Format("値が見つかりません　値[ \"{0}\"", eventName));
            }

            EventClass findEvent = tmpArray[0];
            findEvent.Event.Invoke();
        }

        public void InvokeSetupAssets()
        {
            OnSetup.Invoke();
        }

        public void InvokeStartAssets()
        {
            OnStart.Invoke();
        }

        public void InvokeUpdateAssets()
        {
            OnUpdate.Invoke();
        }

        public void InvokeFixedUpdateAssets()
        {
            OnFixedUpdate.Invoke();
        }

        public void InvokeLateUpdateAssets()
        {
            OnLateUpdate.Invoke();
        }

        public void InvokExitAssets()
        {
            OnExit.Invoke();
        }

        private T[] FindObjectOfInterfaces<T>() where T : class
        {
            List<T> list = new List<T>();
            foreach (var n in GameObject.FindObjectsOfType<Component>(true))
            {
                if (n is T component)
                {
                    list.Add(component);
                }
            }

            T[] ret = new T[list.Count];
            int count = 0;
            foreach (T component in list)
            {
                ret[count] = component;
                count++;
            }
            return ret;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AssetsFacade))]
    public class AssetsFacadeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Set Componets"))
            {
                AssetsFacade t = target as AssetsFacade;
                t.SetInspectorUI();
            }
        }
    }
#endif
}
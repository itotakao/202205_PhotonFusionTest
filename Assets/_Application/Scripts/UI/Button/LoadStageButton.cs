using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Application
{
    public class LoadStageButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        StageId loadStageId = StageId.None;

#if UNITY_EDITOR
        public void SetInspectorUI()
        {
            button = GetComponent<Button>();
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(button.onClick, OnPressedButton);
            UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, OnPressedButton);
        }
#endif

        private void OnPressedButton()
        {
            BaseState.LoadStage(loadStageId);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LoadStageButton))]
    public class LoadStageButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Set UI"))
            {
                LoadStageButton t = target as LoadStageButton;
                t.SetInspectorUI();
            }
        }
    }
#endif
}
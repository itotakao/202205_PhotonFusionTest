#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;

namespace _Util
{
    public class GlobalParameterWindow : EditorWindow
    {
        const string FileNamespace = "_Util";
        private Vector2 _scroll;

        [MenuItem("Util/GlobalParameterWindow")]
        private static void ShowWindow()
        {
            GetWindow<GlobalParameterWindow>();
        }

        private void OnGUI()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            GUI.enabled = false;// Read Only

            GlobalParameter g = new GlobalParameter();
            FieldInfo[] infoArray = g.GetType().GetFields();
            foreach (FieldInfo info in infoArray)
            {
                if (info.FieldType.ToString() == $"{FileNamespace}.Label")
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField(info.Name);
                    continue;
                }

                object value = info.GetValue(g);
                Type type = value.GetType();
                switch (type.Name)
                {
                    case "String":
                        EditorGUILayout.TextField(info.Name, (string)value);
                        break;
                    case "Int32":
                        EditorGUILayout.IntField(info.Name, (int)value);
                        break;
                    case "Single":
                        EditorGUILayout.FloatField(info.Name, (float)value);
                        break;
                    case "Boolean":
                        EditorGUILayout.Toggle(info.Name, (bool)value);
                        break;
                    default:
                        Debug.Log(type.Name);
                        break;
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
#endif
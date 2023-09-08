using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Text;
using UnityXFrame.Core.Parser;
using XFrame.Core;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace UnityXFrame.Editor
{
    public class NameEditor : EditorWindow
    {
        private GUIStyle m_Style1 => GUI.skin.customStyles[205];
        private GUIStyle m_Style2 => GUI.skin.customStyles[487];

        private string m_SrcPath;
        private string m_DstPath;
        private SerializedObject m_SerObj;

        [SerializeField] private List<NameInfo> m_Patterns;
        private SerializedProperty m_PatternsProp;

        private void OnEnable()
        {
            m_SerObj = new SerializedObject(this);
            m_PatternsProp = m_SerObj.FindProperty("m_Patterns");
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical(m_Style1);
            EditorGUILayout.LabelField("Name", m_Style2);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Source Path");
            m_SrcPath = EditorGUILayout.TextField(m_SrcPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Dest Path");
            m_DstPath = EditorGUILayout.TextField(m_DstPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(m_PatternsProp);
            if (GUILayout.Button("Execute"))
            {
                m_SerObj.ApplyModifiedProperties();
                if (m_Patterns != null && m_Patterns.Count > 0)
                    InnerCheckName();
            }

            GUILayout.EndVertical();
        }

        private void InnerCheckName()
        {
            if (string.IsNullOrEmpty(m_SrcPath))
            {
                EditorLog.Debug($"source path {m_SrcPath} is null");
                return;
            }

            if (!Directory.Exists(m_SrcPath))
            {
                EditorLog.Debug($"source path {m_SrcPath} is not exist");
                return;
            }

            foreach (string file in Directory.EnumerateFiles(m_SrcPath))
            {
                string newFile = InnerMatch(file);
            }
        }

        private string InnerMatch(string file)
        {
            if (Path.GetExtension(file) == ".meta")
                return null;
            /*string name = Path.GetFileNameWithoutExtension(file);
            StringBuilder result = new StringBuilder();
            bool isFirst = true;
            foreach (NameInfo info in m_Patterns)
            {
                Match match = Regex.Match(name, info.Pattern);
                if (match.Success)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        result.Append(Name.SPLIT2);
                    }
                    Group group = match.Groups[0];
                    result.Append($"{info.Type}@");
                    if (info.ValueIsInt)
                    {
                        IntParser.TryParse(info.Value, out int value);
                        result.Append(value.ToString());
                    }
                    else
                    {
                        result.Append(string.Format(info.Value, group.Value);
                    }
                }
                else
                {
                    return null;
                }
            }
            Debug.LogWarning(result.ToString());
            return result.ToString();*/
            return null;
        }
    }
}

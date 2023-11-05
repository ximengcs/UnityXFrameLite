using System;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;
using System.Collections.Generic;
using XFrame.Modules.Reflection;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class DebugEditor : DataEditorBase
        {
            private const string DEBUG = "CONSOLE";
            private TypeSystem m_LogHelperTypes;
            private Type[] m_Types;
            private string[] m_LogHelperTypeNames;
            private int m_LogHelperTypeIndex;
            private Vector2 m_LogScrollPos;
            private GUISkin m_DebuggerSkin;
            private bool m_MoreColorDetail;

            protected override void OnInit()
            {
                m_MoreColorDetail = false;
                m_LogHelperTypes = m_Inst.m_FrameCore.GetModule<TypeModule>().GetOrNew<XFrame.Modules.Diagnotics.ILogger>();
                m_Types = m_LogHelperTypes.ToArray();
                m_LogHelperTypeNames = new string[m_Types.Length];
                for (int i = 0; i < m_Types.Length; i++)
                {
                    string name = m_Types[i].Name;
                    m_LogHelperTypeNames[i] = name;
                    if (name == m_Data.ResMode)
                        m_LogHelperTypeIndex = i;
                }

                if (string.IsNullOrEmpty(m_Data.Logger) && m_LogHelperTypeNames.Length > 0)
                    InnerSelect(0);

                m_DebuggerSkin = m_Data.DebuggerSkin;
            }

            private bool InnerIsDebug()
            {
                return Utility.ContainsSymbol(DEBUG);
            }

            private void InnerSaveDebug(bool debug)
            {
                if (debug)
                {
                    if (!Utility.ContainsSymbol(DEBUG))
                        Utility.AddSymbol(DEBUG);
                }
                else
                {
                    if (Utility.ContainsSymbol(DEBUG))
                        Utility.RemoveSymbol(DEBUG);
                }
            }

            public override void OnUpdate()
            {
                #region DebuggerSkin
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Console");
                int old = InnerIsDebug() ? 0 : 1;
                int isDebug = GUILayout.Toolbar(old, new string[] { "On", "Off" });
                if (isDebug != old)
                    InnerSaveDebug(isDebug == 0 ? true : false);
                m_DebuggerSkin = (GUISkin)EditorGUILayout.ObjectField(m_DebuggerSkin, typeof(GUISkin), false);

                if (m_DebuggerSkin != null && m_DebuggerSkin != m_Data.DebuggerSkin)
                {
                    m_Data.DebuggerSkin = m_DebuggerSkin;
                    EditorUtility.SetDirty(m_Data);
                }
                EditorGUILayout.EndHorizontal();
                #endregion

                #region Logger
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Logger Helper");
                int index = m_LogHelperTypeIndex;
                m_LogHelperTypeIndex = EditorGUILayout.Popup(m_LogHelperTypeIndex, m_LogHelperTypeNames);
                if (index != m_LogHelperTypeIndex)
                    InnerSelect(m_LogHelperTypeIndex);
                EditorGUILayout.EndHorizontal();
                #endregion

                #region All Color Select
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                Utility.Lable("Log Colors");
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.customStyles[40]);
                List<DebugColor> colors = m_Data.LogMark;
                #region Calculate Select
                EditorGUILayout.BeginHorizontal();
                bool all = true;
                bool dirty = false;
                if (colors == null)
                {
                    colors = new List<DebugColor>();
                    m_Data.LogMark = colors;
                }
                foreach (DebugColor data in colors)
                {
                    if (data.Value)
                        data.Color = EditorGUILayout.ColorField(new GUIContent(""), data.Color, false, false, false, GUILayout.Width(20));
                    if (!data.Value)
                        all = false;
                }
                bool olds = all;
                all = EditorGUILayout.Toggle(all, GUILayout.Width(15));
                if (olds != all)
                    dirty = true;

                if (GUILayout.Button("x", GUILayout.Width(20)))
                    colors.Clear();

                GUILayout.FlexibleSpace();
                m_MoreColorDetail = Utility.Toggle(m_MoreColorDetail);
                EditorGUILayout.EndHorizontal();
                #endregion


                if (m_MoreColorDetail)
                {
                    m_LogScrollPos = EditorGUILayout.BeginScrollView(m_LogScrollPos);
                    List<int> willDel = new List<int>();
                    for (int i = 0; i < colors.Count; i++)
                    {
                        DebugColor logMark = colors[i];
                        if (dirty) logMark.Value = all;
                        EditorGUILayout.BeginHorizontal();
                        logMark.Key = EditorGUILayout.TextField(logMark.Key);
                        logMark.Color = EditorGUILayout.ColorField(logMark.Color, GUILayout.Width(40));
                        logMark.Value = EditorGUILayout.Toggle(logMark.Value);
                        if (GUILayout.Button("x"))
                            willDel.Add(i);
                        EditorGUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("+"))
                    {
                        DebugColor data = new DebugColor();
                        data.Value = true;
                        colors.Add(data);
                    }

                    foreach (int i in willDel)
                        colors.RemoveAt(i);
                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    foreach (DebugColor data in colors)
                        if (dirty) data.Value = all;
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                #endregion
            }

            private void InnerSelect(int index)
            {
                m_LogHelperTypeIndex = index;
                m_Data.Logger = m_Types[m_LogHelperTypeIndex].FullName;
                EditorUtility.SetDirty(m_Data);
            }
        }
    }
}

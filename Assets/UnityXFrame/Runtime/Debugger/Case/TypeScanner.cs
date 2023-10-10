using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using XFrame.Core;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugHelp("browse types")]
    [DebugWindow(-998, "Types")]
    public class TypeScanner : IDebugWindow
    {
        private class TypesInfo
        {
            public bool Show;
            public string Pattern;
            public StringBuilder List;
            public Vector2 ScrollPos;
            public int Count;

            public TypesInfo(string pattern)
            {
                Pattern = pattern;
                Show = true;
                List = new StringBuilder();
                ScrollPos = new Vector2();
                Count = 0;
            }
        }

        private Vector2 m_Pos;
        private GUIStyle m_BoxStyle;
        private string m_Pattern;
        private Dictionary<string, TypesInfo> m_Types;

        public void OnAwake()
        {
            m_Types = new Dictionary<string, TypesInfo>();
            m_BoxStyle = new GUIStyle(GUI.skin.box);
            m_BoxStyle.fontSize = 30;

            Debugger debugger = (Debugger)Global.Debugger;
            debugger.FitStyle(m_BoxStyle);
        }

        public void OnDraw()
        {
            GUILayout.BeginHorizontal();
            m_Pattern = DebugGUI.TextField(m_Pattern);
            if (DebugGUI.Button("+", DebugGUI.Width(100)))
            {
                if (m_Types.ContainsKey(m_Pattern))
                    m_Types.Remove(m_Pattern);
                TypesInfo info = new TypesInfo(m_Pattern);
                InnerRefresh(info);
                m_Types.Add(m_Pattern, info);
            }
            GUILayout.EndHorizontal();
            if (DebugGUI.Button("Refresh"))
            {
                foreach (TypesInfo info in m_Types.Values)
                    InnerRefresh(info);
            }

            m_Pos = DebugGUI.BeginScrollView(m_Pos);
            foreach (var info in new List<TypesInfo>(m_Types.Values))
            {
                GUILayout.BeginHorizontal();
                info.Show = DebugGUI.Toggle(info.Show, $"{info.Pattern}({info.Count})");
                if (DebugGUI.Button("x", DebugGUI.Width(100)))
                {
                    m_Types.Remove(info.Pattern);
                    continue;
                }
                GUILayout.EndHorizontal();
                if (info.Show)
                {
                    GUIContent list = new GUIContent(info.List.ToString());
                    info.ScrollPos = DebugGUI.BeginScrollView(info.ScrollPos, DebugGUI.Height(m_BoxStyle.fontSize * info.Count));
                    GUILayout.Box(list, m_BoxStyle);
                    GUILayout.EndScrollView();
                }
            }
            GUILayout.EndScrollView();
        }

        public void Dispose()
        {

        }

        private void InnerRefresh(TypesInfo info)
        {
            info.List.Clear();
            Type[] types = Global.Type.GetAllType();
            foreach (Type type in types)
            {
                string name = type.Name;
                if (name.StartsWith(info.Pattern))
                {
                    info.Count++;
                    info.List.AppendLine(name);
                }
            }
        }
    }
}

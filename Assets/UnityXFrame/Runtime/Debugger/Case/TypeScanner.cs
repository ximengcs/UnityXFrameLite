using System;
using System.Text;
using UnityEngine;
using XFrame.Modules.XType;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugHelp("browse types")]
    [DebugWindow(-998, "Types")]
    public class TypeScanner : IDebugWindow
    {
        private Vector2 m_Pos;
        private StringBuilder m_Str;

        public void OnAwake()
        {
            if (m_Str == null)
            {
                m_Str = new StringBuilder();
                InnerRefresh();
            }
        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Refresh"))
                InnerRefresh();

            GUILayout.BeginHorizontal();
            DebugGUI.Button("Q", GUILayout.Width(30));
            DebugGUI.Button("W", GUILayout.Width(30));
            DebugGUI.Button("E", GUILayout.Width(30));
            DebugGUI.Button("R", GUILayout.Width(30));
            DebugGUI.Button("T", GUILayout.Width(30));
            DebugGUI.Button("Y", GUILayout.Width(30));
            DebugGUI.Button("U", GUILayout.Width(30));
            DebugGUI.Button("I", GUILayout.Width(30));
            DebugGUI.Button("O", GUILayout.Width(30));
            GUILayout.EndHorizontal();

            //m_Pos = DebugGUI.BeginScrollView(m_Pos);
            //GUILayout.Box(new GUIContent(m_Str.ToString()));
            //GUILayout.EndScrollView();
        }

        public void Dispose()
        {

        }

        private void InnerRefresh()
        {
            m_Str.Clear();
            Type[] types = TypeModule.Inst.GetAllType();
            foreach (Type type in types)
            {
                m_Str.AppendLine(type.Name);
            }
        }
    }
}

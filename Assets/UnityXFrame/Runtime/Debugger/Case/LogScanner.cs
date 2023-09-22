using System.Text;
using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugHelp("log capture")]
    [DebugWindow(-997, "Log", true)]
    public class LogScanner : IDebugWindow
    {
        private StringBuilder m_Common;
        private StringBuilder m_Warning;
        private StringBuilder m_Error;

        private Vector2 m_CommonScrollPos;
        private Vector2 m_WarningScrollPos;
        private Vector2 m_ErrorScrollPos;

        private int m_TabIndex;

        public void OnAwake()
        {
            m_Common = new StringBuilder();
            m_Warning = new StringBuilder();
            m_Error = new StringBuilder();
            Application.logMessageReceived += InternalLogCallback;
            Log.ConsumeWaitQueue();
            Log.ToQueue = false;
        }

        public void OnDraw()
        {
            m_TabIndex = DebugGUI.Toolbar(m_TabIndex, new string[] { "Debug", "Warning", "Error" });
            switch (m_TabIndex)
            {
                case 0: InternalDrawKind(m_Common, ref m_CommonScrollPos); break;
                case 1: InternalDrawKind(m_Warning, ref m_WarningScrollPos); break;
                case 2: InternalDrawKind(m_Error, ref m_ErrorScrollPos); break;
            }
        }

        public void Dispose()
        {
            Application.logMessageReceived -= InternalLogCallback;
            m_Common = null;
            m_Warning = null;
            m_Error = null;
        }

        private void InternalDrawKind(StringBuilder content, ref Vector2 scrollPos)
        {
            GUILayout.BeginHorizontal();
            if (DebugGUI.Button("Copy"))
                GUIUtility.systemCopyBuffer = content.ToString();
            if (DebugGUI.Button("Clear"))
                content.Clear();
            GUILayout.EndHorizontal();
            scrollPos = DebugGUI.BeginScrollView(scrollPos);
            GUILayout.Box(new GUIContent(content.ToString()));
            GUILayout.EndScrollView();
        }

        private void InternalLogCallback(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    m_Common.Append(condition);
                    m_Common.Append("\n\n");
                    break;
                case LogType.Warning:
                    m_Warning.Append("<color=#CC9A06>");
                    m_Warning.Append(condition);
                    m_Warning.Append("\n");
                    m_Warning.Append(stackTrace);
                    m_Warning.Append("</color>\n\n");
                    break;
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    Module.Debugger.SetTip(this, "LogScanner has new error");
                    m_Error.Append("<color=#CC423B>");
                    m_Error.Append(condition);
                    m_Error.Append("\n");
                    m_Error.Append(stackTrace);
                    m_Error.Append("</color>\n\n");
                    break;
            }
        }
    }
}
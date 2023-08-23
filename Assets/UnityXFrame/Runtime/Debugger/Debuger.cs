using System;
using UnityEngine;
using XFrame.Core;
using System.Reflection;
using XFrame.Modules.Times;
using XFrame.Modules.XType;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debuger : SingletonModule<Debuger>, IGUI
    {
        #region Internal Field
        private const int WIDTH = 1080;
        private const int HEIGHT = 1920;
        internal float m_FitWidth;
        internal float m_FitHeight;

        private GUISkin Skin;
        private GUIStyle m_TitleStyle;
        private GUIStyle m_CloseButtonStyle;
        private GUIStyle m_EnterButtonStyle;
        private GUIStyle m_TipTitleStyle;
        private GUIStyle m_TipContentStyle;
        private GUIStyle m_DebugArea;
        private GUIStyle m_MenuArea;
        private GUIStyle m_ContentArea;
        private GUIStyle m_HelpWindowStyle;
        private GUIStyle m_MenuButton;
        private GUIStyle m_CmdRunButton;
        private GUIStyle m_CmdContentStyle;

        private bool m_IsOpen;
        private bool m_HelpOpen;
        private Rect m_RootRect;
        private float m_RootHeight;
        private bool m_OnGUIInit;
        private Rect m_HelpRect;
        private Vector2 m_ContentPos;
        private Vector2 m_DebugMenuPos;
        private List<WindowInfo> m_Windows;
        private WindowInfo m_Current;

        private float m_ExtraHeight;
        private Vector2 m_ScrollPos;
        private bool m_ShowFps;
        private TweenModule m_TweenModule;
        private HashSet<int> m_TipNewMsg;
        private bool m_AlwaysTip;
        private string m_Tip;
        private string m_Cmd;
        private CDTimer m_Timer;
        private string m_EnterText;
        private EventSystem m_EventSytem;

        private const string TITLE = "Console";
        private const int TIP_CD_KEY = 0;
        private const int TIP_CD = 3;
        #endregion
        public void SetTip(IDebugWindow from, string content, string color = null)
        {
            SetTip(from.GetHashCode(), content, color);
        }

        public void SetTip(int instanceId, string content, string color = null)
        {
            if (!string.IsNullOrEmpty(color))
                content = $"<color=#{color.Trim('#')}>{content}</color>";

            m_Tip = content;
            m_Timer.Reset(TIP_CD_KEY);
            m_Timer.Check(TIP_CD_KEY, true);

            if (instanceId != -1 && !m_TipNewMsg.Contains(instanceId))
                m_TipNewMsg.Add(instanceId);
        }
        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_ShowFps = false;
            m_OnGUIInit = false;
            Skin = ScriptableObject.Instantiate(Init.Inst.Data.DebuggerSkin);
            m_MenuButton = Skin.button;
            m_CloseButtonStyle = Skin.customStyles[0];
            m_TitleStyle = Skin.customStyles[1];
            m_EnterButtonStyle = Skin.customStyles[2];

            DebugGUI.Style = new DebugStyle();
            DebugGUI.Style.Skin = Skin;
            DebugGUI.Style.HorizontalScrollbar = Skin.horizontalScrollbar;
            DebugGUI.Style.VerticalScrollbar = Skin.verticalScrollbar;
            DebugGUI.Style.Button = Skin.customStyles[3];
            DebugGUI.Style.Text = Skin.customStyles[4];
            DebugGUI.Style.Lable = Skin.customStyles[5];
            DebugGUI.Style.TextArea = Skin.customStyles[6];
            m_TipTitleStyle = Skin.customStyles[7];
            m_TipContentStyle = Skin.customStyles[8];
            m_DebugArea = Skin.customStyles[9];
            DebugGUI.Style.Toolbar = Skin.customStyles[10];
            m_MenuArea = Skin.customStyles[11];
            m_ContentArea = Skin.customStyles[12];
            m_HelpWindowStyle = Skin.customStyles[13];
            DebugGUI.Style.ProgressSlider = Skin.customStyles[14];
            DebugGUI.Style.ProgressThumb = Skin.customStyles[15];
            m_CmdRunButton = Skin.customStyles[16];
            m_CmdContentStyle = Skin.customStyles[17];

            m_TweenModule = new TweenModule();
            m_Timer = CDTimer.Create();
            m_Timer.Record(TIP_CD_KEY, TIP_CD);
            m_TipNewMsg = new HashSet<int>();

            m_EnterText = TITLE;
            m_Windows = new List<WindowInfo>();
            InternalLoadInst();
            InnerInitCmd();

            if (m_Windows.Count > 0)
                InternalSelectMenu(m_Windows[0]);
        }

        private void InnerFixScreen()
        {
            m_FitWidth = Screen.width / (float)WIDTH;
            m_FitHeight = Screen.height / (float)HEIGHT;

            FitStyle(m_CloseButtonStyle);
            FitStyle(m_TitleStyle);
            FitStyle(m_EnterButtonStyle);
            FitStyle(m_TipTitleStyle);
            FitStyle(m_TipContentStyle);
            FitStyle(m_CmdContentStyle);
            FitStyle(m_HelpWindowStyle);
            FitStyle(m_DebugArea);
            FitStyle(m_MenuArea);
            FitStyle(m_ContentArea);
            FitStyle(m_MenuButton);
            FitStyle(m_CmdRunButton);
            FitStyle(DebugGUI.Style.HorizontalScrollbar);
            FitStyle(DebugGUI.Style.VerticalScrollbar);
            FitStyle(DebugGUI.Style.Button);
            FitStyle(DebugGUI.Style.Text);
            FitStyle(DebugGUI.Style.Lable);
            FitStyle(DebugGUI.Style.TextArea);
            FitStyle(DebugGUI.Style.Toolbar);
            FitStyle(DebugGUI.Style.ProgressSlider);
            FitStyle(DebugGUI.Style.ProgressThumb);
            FitStyle(Skin.verticalScrollbarThumb);
            FitStyle(Skin.horizontalScrollbarThumb);
            FitStyle(Skin.box);
        }

        public float FitWidth(float width)
        {
            return m_FitWidth * width;
        }

        public float FitHeight(float height)
        {
            return m_FitHeight * height;
        }

        public void FitStyle(GUIStyle style)
        {
            if (style.fontSize != 0)
                style.fontSize = (int)(m_FitHeight * style.fontSize);
            if (style.fixedHeight != 0)
                style.fixedHeight *= m_FitHeight;
            if (style.fixedWidth != 0)
                style.fixedWidth *= m_FitWidth;
            style.margin.right = (int)(style.margin.right * m_FitWidth);
            style.margin.left = (int)(style.margin.left * m_FitWidth);
            style.margin.top = (int)(style.margin.top * m_FitHeight);
            style.margin.bottom = (int)(style.margin.bottom * m_FitHeight);
            style.padding.right = (int)(style.padding.right * m_FitWidth);
            style.padding.left = (int)(style.padding.left * m_FitWidth);
            style.padding.top = (int)(style.padding.top * m_FitHeight);
            style.padding.bottom = (int)(style.padding.bottom * m_FitHeight);
        }

        private void InnerGUIInit()
        {
            GUI.skin.verticalScrollbarThumb = Skin.verticalScrollbarThumb;
            GUI.skin.horizontalScrollbarThumb = Skin.horizontalScrollbarThumb;
            GUI.skin.box = Skin.box;
            Skin.window.fixedWidth = Screen.width;
            Skin.window.fixedHeight = Mathf.Min(Skin.window.fixedHeight, Screen.height);
            m_HelpRect.y = Skin.window.fixedHeight;
            InnerFixScreen();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (WindowInfo info in m_Windows)
                info.Window.Dispose();
            m_Windows = null;
        }

        public void OnGUI()
        {
            if (!m_OnGUIInit)
            {
                m_OnGUIInit = true;
                InnerGUIInit();
            }

            InternalCheckInGUI();
            if (m_IsOpen)
            {
                m_RootRect = default;
                m_RootRect = GUILayout.Window(0, m_RootRect, InternalDrawRootWindow, string.Empty, Skin.window);
                if (m_RootRect.height > 0)
                    m_RootHeight = m_RootRect.height;
                if (m_HelpWindowStyle.fixedHeight > 0)
                {
                    m_HelpRect.y = m_RootHeight;
                    m_HelpRect.width = m_RootRect.width;
                    m_HelpRect = GUILayout.Window(1, m_HelpRect, InternalDrawHelpWindow, string.Empty, m_HelpWindowStyle);
                }
                m_TweenModule.OnUpdate();
            }
            else
            {
                string title = m_EnterText;
                if (m_TipNewMsg.Count > 0)
                    title = $"<color=#FF0000>{title}</color>";
                if (GUILayout.Button(title, m_EnterButtonStyle))
                    m_IsOpen = true;
            }

            if (m_ShowFps)
                m_EnterText = InnerCalculateFps();
            else
                m_EnterText = TITLE;
        }
        #endregion

        private string InnerCalculateFps()
        {
            float fps = 1 / Time.deltaTime;
            return string.Format("FPS {0:F2}", fps);
        }

        private void InternalDrawHelpWindow(int windowId)
        {
            if (m_HelpWindowStyle.fixedHeight < m_RootHeight)
                return;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Help", m_TitleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            m_ScrollPos = DebugGUI.BeginScrollView(m_ScrollPos);
            GUILayout.Box(m_Current.HelpInfo, Skin.box);
            GUILayout.EndScrollView();
        }

        #region Internal Implement
        private void InternalCheckInGUI()
        {
            if (m_EventSytem == null)
            {
                m_EventSytem = EventSystem.current;
            }
            else
            {
                Vector3 touchPos = Input.mousePosition;
                touchPos.y = Screen.height - touchPos.y;
                bool enable = m_IsOpen ? !m_RootRect.Contains(touchPos) : true;
                m_EventSytem.enabled = enable;
            }
        }

        private void InternalLoadInst()
        {
            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IDebugWindow>();
            foreach (Type t in typeSys)
                InnerAddWindowInfo(t);
            m_Windows.Sort((info1, info2) => info2.Order - info1.Order);
            TypeModule.Inst.OnTypeChange(InnerNewWindowHandle);
        }

        private void InnerNewWindowHandle()
        {
            HashSet<Type> types = new HashSet<Type>();
            foreach (WindowInfo info in m_Windows)
                types.Add(info.Window.GetType());

            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IDebugWindow>();
            foreach (Type t in typeSys)
            {
                if (!types.Contains(t))
                    InnerAddWindowInfo(t);
            }
            m_Windows.Sort((info1, info2) => info2.Order - info1.Order);
        }

        private void InnerAddWindowInfo(Type type)
        {
            DebugWindowAttribute atr = type.GetCustomAttribute<DebugWindowAttribute>();

            WindowInfo info = new WindowInfo();
            if (atr != null)
            {
                info.Name = atr.Name;
                info.AlwaysRun = atr.AlwaysRun;
                info.Order = atr.Order;
            }
            else
            {
                info.Name = default;
                info.AlwaysRun = default;
                info.Order = default;
            }
            if (string.IsNullOrEmpty(info.Name))
                info.Name = type.Name.Replace("Case", string.Empty);
            if (info.Name.Length > 10)
                info.Name = info.Name.Substring(0, 10);

            DebugHelpAttribute helpAtr = type.GetCustomAttribute<DebugHelpAttribute>();
            if (helpAtr != null)
                info.HelpInfo = helpAtr.Content;
            else
                info.HelpInfo = "No help information";

            IDebugWindow window = Activator.CreateInstance(type) as IDebugWindow;
            info.Window = window;
            m_Windows.Add(info);

            if (info.AlwaysRun)
                window.OnAwake();
        }

        internal void InnerSwitchFPS(bool open)
        {
            m_ShowFps = open;
        }

        internal void InnerClose()
        {
            m_IsOpen = false;
            m_HelpOpen = false;
        }

        private void InternalDrawRootWindow(int windowId)
        {
            GUILayout.BeginHorizontal();
            if (m_ShowFps)
                DebugGUI.Label(InnerCalculateFps());
            else
                DebugGUI.Label("FPS");
            m_ShowFps = DebugGUI.Power(m_ShowFps);

            GUILayout.FlexibleSpace();
            GUILayout.Label(TITLE, m_TitleStyle);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("X", m_CloseButtonStyle))
                InnerClose();

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(m_DebugArea);
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(m_MenuArea);

            m_DebugMenuPos = DebugGUI.BeginScrollView(m_DebugMenuPos);
            foreach (WindowInfo windowInfo in m_Windows)
            {
                string title = windowInfo.Name;
                int code = windowInfo.Window.GetHashCode();
                if (m_TipNewMsg.Contains(code))
                    title = $"<color=#FF0000>{title}</color>";
                if (windowInfo.Window == m_Current.Window)
                    title = $"<color=#2A89FF>{title}</color>";

                if (GUILayout.Button(title, m_MenuButton))
                    InternalSelectMenu(windowInfo);

                code = m_Current.Window.GetHashCode();
                if (m_TipNewMsg.Contains(code))
                    m_TipNewMsg.Remove(code);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(m_ContentArea);
            m_ContentPos = DebugGUI.BeginScrollView(m_ContentPos);
            m_Current.Window?.OnDraw();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("?", m_TipTitleStyle))
            {
                m_HelpOpen = !m_HelpOpen;
                float target = m_HelpOpen ? m_RootHeight : 0;
                m_TweenModule.Do("?", target, 0.1f,
                    () => m_HelpWindowStyle.fixedHeight,
                    (v) => m_HelpWindowStyle.fixedHeight = v);
            }
            bool alwaysTip = GUILayout.Toggle(m_AlwaysTip, "Tip", m_TipTitleStyle);
            if (alwaysTip != m_AlwaysTip)
            {
                Debuger.Tip($"tip mode change to {(alwaysTip ? "always" : "cd")}", Color.yellow);
                m_AlwaysTip = alwaysTip;
            }
            if (!m_AlwaysTip && m_Timer.Check(TIP_CD_KEY, true))
                m_Tip = string.Empty;
            GUILayout.Label(m_Tip, m_TipContentStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            float cmdHeight = m_CmdRunButton.fixedHeight;
            if (!string.IsNullOrEmpty(m_Cmd))
            {
                cmdHeight = Mathf.Max(cmdHeight, m_Cmd.Split('\n').Length * m_CmdContentStyle.lineHeight);
            }
            m_Cmd = GUILayout.TextArea(m_Cmd, m_CmdContentStyle, GUILayout.Height(cmdHeight));
            if (GUILayout.Button("RUN", m_CmdRunButton))
                InnerRunCmd(m_Cmd);
            GUILayout.EndHorizontal();
        }

        private void InternalSelectMenu(WindowInfo info)
        {
            if (m_Current.Window != null && !m_Current.AlwaysRun)
                m_Current.Window?.Dispose();
            m_Current = info;
            if (m_Current.Window != null && !m_Current.AlwaysRun)
                m_Current.Window.OnAwake();
        }

        private struct WindowInfo
        {
            public string Name;
            public int Order;
            public bool AlwaysRun;
            public IDebugWindow Window;
            public string HelpInfo;
        }
        #endregion
    }
}
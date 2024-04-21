using System;
using UnityEngine;
using XFrame.Core;
using System.Reflection;
using XFrame.Modules.Times;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Modules.Reflection;

namespace UnityXFrame.Core.Diagnotics
{
    [XType(typeof(IDebugger))]
    public partial class Debugger : ModuleBase, IDebugger
    {
        #region Internal Field
        private const int WIDTH = 1080;
        private const int HEIGHT = 1920;
        private float m_FitWidth;
        private float m_FitHeight;

        private GUISkin Skin;
        private GUIStyle m_TitleStyle;
        private GUIStyle m_CloseButtonStyle;
        private GUIStyle m_EnterButtonStyle;
        private GUIStyle m_HelpButtonStyle;
        private GUIStyle m_TipTitleStyle;
        private GUIStyle m_TipContentStyle;
        private GUIStyle m_DebugArea;
        private GUIStyle m_MenuArea;
        private GUIStyle m_ContentArea;
        private GUIStyle m_HelpWindowStyle;
        private GUIStyle m_MenuButton;
        private GUIStyle m_CollapseButton;
        private GUIStyle m_CmdRunButton;
        private GUIStyle m_CmdContentStyle;

        private bool m_IsOpen;
        private bool m_Collapsing;
        private bool m_HelpOpen;
        private Rect m_RootRect;
        private float m_RootHeight;
        private float m_HelpHeight;
        private bool m_OnGUIInit;
        private Rect m_HelpRect;
        private Vector2 m_ContentPos;
        private Vector2 m_DebugMenuPos;
        private List<WindowInfo> m_Windows;
        private WindowInfo m_Current;

        private Vector2 m_HelpScrollPos;
        private int m_HelpSelect;
        private bool m_ShowFps;
        private HashSet<int> m_TipNewMsg;
        private bool m_AlwaysTip;
        private string m_Tip;
        private string m_Cmd;
        private CDTimer m_Timer;
        private string m_EnterText;
        private EventSystem m_EventSytem;

        private float m_FpsValue;
        private float m_FpsAmount;
        private int m_FpsCurTime;

        private const string TITLE = "Console";
        private const int TIP_CD_KEY = 0;
        private const int TIP_CD = 3;
        #endregion

        public int FpsTimeGap { get; set; }

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

            FpsTimeGap = 30;
            m_ShowFps = false;
            m_OnGUIInit = false;
            Skin = ScriptableObject.Instantiate(Init.Inst.Data.DebuggerSkin);
            m_MenuButton = Skin.button;
            m_CloseButtonStyle = Skin.customStyles[0];
            m_TitleStyle = Skin.customStyles[1];
            m_EnterButtonStyle = Skin.customStyles[2];

            DebugGUI.Style = new DebugStyle();
            DebugGUI.Style.Skin = Skin;
            DebugGUI.Style.HorizontalSlider = Skin.horizontalSlider;
            DebugGUI.Style.HorizontalSliderThumb = Skin.horizontalSliderThumb;
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
            DebugGUI.Style.Rect = Skin.customStyles[18];
            DebugGUI.Style.Title = Skin.customStyles[19];
            m_CollapseButton = Skin.customStyles[20];
            m_HelpButtonStyle = Skin.customStyles[21];
            DebugGUI.Style.Button2 = Skin.customStyles[22];

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
            FitStyle(m_CloseButtonStyle);
            FitStyle(m_TitleStyle);
            FitStyle(m_EnterButtonStyle);
            FitStyle(m_TipTitleStyle);
            FitStyle(m_HelpButtonStyle);
            FitStyle(m_TipContentStyle);
            FitStyle(m_CmdContentStyle);
            FitStyle(m_HelpWindowStyle);
            FitStyle(m_DebugArea);
            FitStyle(m_MenuArea);
            FitStyle(m_ContentArea);
            FitStyle(m_CollapseButton);
            FitStyle(m_MenuButton);
            FitStyle(m_CmdRunButton);
            FitStyle(DebugGUI.Style.HorizontalSlider);
            FitStyle(DebugGUI.Style.HorizontalSliderThumb);
            FitStyle(DebugGUI.Style.HorizontalScrollbar);
            FitStyle(DebugGUI.Style.VerticalScrollbar);
            FitStyle(DebugGUI.Style.Button);
            FitStyle(DebugGUI.Style.Text);
            FitStyle(DebugGUI.Style.Lable);
            FitStyle(DebugGUI.Style.TextArea);
            FitStyle(DebugGUI.Style.Toolbar);
            FitStyle(DebugGUI.Style.ProgressSlider);
            FitStyle(DebugGUI.Style.ProgressThumb);
            FitStyle(DebugGUI.Style.Rect);
            FitStyle(DebugGUI.Style.Title);
            FitStyle(DebugGUI.Style.Button2);
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
            m_FitWidth = Screen.width / (float)WIDTH;
            m_FitHeight = Screen.height / (float)HEIGHT;

            GUI.skin.verticalScrollbarThumb = Skin.verticalScrollbarThumb;
            GUI.skin.horizontalScrollbarThumb = Skin.horizontalScrollbarThumb;
            GUI.skin.box = Skin.box;
            Skin.window.fixedWidth = Screen.width;
            Skin.window.fixedHeight = Mathf.Min(Skin.window.fixedHeight, Screen.height);
            m_HelpRect.y = Skin.window.fixedHeight;
            m_HelpHeight = FitHeight(m_HelpWindowStyle.fixedHeight);
            m_HelpWindowStyle.fixedHeight = 0;
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
            if (m_FpsValue == 0)
                m_FpsValue = 1 / Time.unscaledDeltaTime;
            m_FpsAmount += Time.unscaledDeltaTime;
            m_FpsCurTime++;
            if (m_FpsCurTime >= FpsTimeGap)
            {
                m_FpsValue = 1 / (m_FpsAmount / FpsTimeGap);
                m_FpsCurTime = 0;
                m_FpsAmount = 0;
            }
            return string.Format("FPS {0:F2}", m_FpsValue);
        }

        private void InternalDrawHelpWindow(int windowId)
        {
            if (m_HelpWindowStyle.fixedHeight < m_HelpHeight)
                return;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Help", m_TitleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            string[] helpTitles = new string[] { "Cmd", "Menu", "Input" };
            m_HelpSelect = Mathf.Clamp(m_HelpSelect, 0, helpTitles.Length);
            m_HelpSelect = DebugGUI.Toolbar(m_HelpSelect, helpTitles);
            GUILayout.EndHorizontal();

            m_HelpScrollPos = DebugGUI.BeginScrollView(m_HelpScrollPos);
            switch (m_HelpSelect)
            {
                case 0: GUILayout.Box(m_CmdHelpInfo, Skin.box); break;
                case 1: GUILayout.Box(m_Current.HelpInfo, Skin.box); break;
                case 2: InnerDrawKeyboard(); break;
            }
            GUILayout.EndScrollView();
        }

        private bool m_CapsLock;
        private string InnerGetAlphabet(char alphabet)
        {
            return m_CapsLock ? alphabet.ToString() : ((char)(alphabet + 32)).ToString();
        }

        private void InnerKeyboardHandler1(char ch, float width)
        {
            if (DebugGUI.Button(ch.ToString(), GUILayout.Width(FitWidth(width))))
                m_Cmd += ch;
        }

        private void InnerKeyboardHandler2(char ch)
        {
            if (DebugGUI.Button(ch.ToString()))
                m_Cmd += ch;
        }

        private void InnerKeyboardHandler3(char ch, float width)
        {
            string chStr = InnerGetAlphabet(ch);
            if (DebugGUI.Button2(chStr, GUILayout.Width(FitWidth(width))))
                m_Cmd += chStr;
        }

        private void InnerDrawKeyboard()
        {
            GUILayout.BeginVertical(Skin.box);

            GUILayout.BeginHorizontal();
            InnerKeyboardHandler1('1', 80);
            InnerKeyboardHandler1('2', 80);
            InnerKeyboardHandler1('3', 80);
            InnerKeyboardHandler1('4', 80);
            InnerKeyboardHandler1('5', 80);
            InnerKeyboardHandler1('6', 80);
            InnerKeyboardHandler1('7', 80);
            InnerKeyboardHandler1('8', 80);
            InnerKeyboardHandler1('9', 80);
            InnerKeyboardHandler1('0', 80);
            InnerKeyboardHandler2('_');
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            InnerKeyboardHandler3('Q', 80);
            InnerKeyboardHandler3('W', 80);
            InnerKeyboardHandler3('E', 80);
            InnerKeyboardHandler3('R', 80);
            InnerKeyboardHandler3('T', 80);
            InnerKeyboardHandler3('Y', 80);
            InnerKeyboardHandler3('U', 80);
            InnerKeyboardHandler3('I', 80);
            InnerKeyboardHandler3('O', 80);
            InnerKeyboardHandler3('P', 80);
            if (DebugGUI.Button("←"))
            {
                if (!string.IsNullOrEmpty(m_Cmd))
                    m_Cmd = m_Cmd.Substring(0, m_Cmd.Length - 1);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            m_CapsLock = DebugGUI.Toggle(m_CapsLock, "■", GUILayout.Width(FitWidth(50)));
            InnerKeyboardHandler3('A', 80);
            InnerKeyboardHandler3('S', 80);
            InnerKeyboardHandler3('D', 80);
            InnerKeyboardHandler3('F', 80);
            InnerKeyboardHandler3('G', 80);
            InnerKeyboardHandler3('H', 80);
            InnerKeyboardHandler3('J', 80);
            InnerKeyboardHandler3('K', 80);
            InnerKeyboardHandler3('L', 80);
            if (DebugGUI.Button("ENT"))
                m_Cmd += '\n';
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (DebugGUI.Button("CLR", GUILayout.Width(FitWidth(90))))
                InnerClearCmd();
            InnerKeyboardHandler3('Z', 80);
            InnerKeyboardHandler3('X', 80);
            InnerKeyboardHandler3('C', 80);
            InnerKeyboardHandler3('V', 80);
            InnerKeyboardHandler3('B', 80);
            InnerKeyboardHandler3('N', 80);
            InnerKeyboardHandler3('M', 80);
            InnerKeyboardHandler1(',', 80);
            InnerKeyboardHandler1('.', 80);
            InnerKeyboardHandler2('?');
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            InnerKeyboardHandler1('\"', 80);
            InnerKeyboardHandler1('|', 80);
            InnerKeyboardHandler1('<', 80);
            if (DebugGUI.Button("SPACE", GUILayout.Width(FitWidth(230))))
                m_Cmd += " ";
            InnerKeyboardHandler1('>', 80);
            InnerKeyboardHandler1('+', 80);
            InnerKeyboardHandler1('-', 80);
            InnerKeyboardHandler1('*', 80);
            InnerKeyboardHandler2('/');
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            InnerKeyboardHandler1('=', 80);
            InnerKeyboardHandler1('\\', 80);
            InnerKeyboardHandler1('~', 80);
            InnerKeyboardHandler1('!', 80);
            InnerKeyboardHandler1('@', 80);
            InnerKeyboardHandler1('#', 80);
            InnerKeyboardHandler1('$', 80);
            InnerKeyboardHandler1('%', 80);
            InnerKeyboardHandler1('(', 80);
            InnerKeyboardHandler1(')', 80);
            InnerKeyboardHandler2(';');
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
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
                enable = m_HelpOpen ? !m_HelpRect.Contains(touchPos) : enable;
                m_EventSytem.enabled = enable;
            }
        }

        private void InternalLoadInst()
        {
            TypeSystem typeSys = Domain.TypeModule.GetOrNew<IDebugWindow>();
            foreach (Type t in typeSys)
                InnerAddWindowInfo(t);
            m_Windows.Sort((info1, info2) => info2.Order - info1.Order);
            Domain.TypeModule.OnTypeChange(InnerNewWindowHandle);
        }

        private void InnerNewWindowHandle()
        {
            HashSet<Type> types = new HashSet<Type>();
            foreach (WindowInfo info in m_Windows)
                types.Add(info.Window.GetType());

            TypeSystem typeSys = Domain.TypeModule.GetOrNew<IDebugWindow>();
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

        internal void InnerCollapse(int collapse = -1)
        {
            if (collapse == -1)
                m_Collapsing = !m_Collapsing;
            else
                m_Collapsing = collapse != 0;
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

            if (GUILayout.Button("?", m_HelpButtonStyle))
            {
                m_HelpOpen = !m_HelpOpen;
                float target = m_HelpOpen ? m_HelpHeight : 0;
                m_HelpWindowStyle.fixedHeight = target;
            }
            if (GUILayout.Button(m_Collapsing ? ">" : "v", m_CollapseButton))
                InnerCollapse();
            if (GUILayout.Button("X", m_CloseButtonStyle))
                InnerClose();

            GUILayout.EndHorizontal();

            if (!m_Collapsing)
            {
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

                #region Tip
                GUILayout.BeginHorizontal();
                bool alwaysTip = GUILayout.Toggle(m_AlwaysTip, "Tip", m_TipTitleStyle);
                if (alwaysTip != m_AlwaysTip)
                {
                    SetTip($"tip mode change to {(alwaysTip ? "always" : "cd")}", Color.yellow);
                    m_AlwaysTip = alwaysTip;
                }
                if (!m_AlwaysTip && m_Timer.Check(TIP_CD_KEY, true))
                    m_Tip = string.Empty;
                GUILayout.Label(m_Tip, m_TipContentStyle);
                GUILayout.EndHorizontal();
                #endregion 
            }

            GUILayout.BeginHorizontal();

            float cmdHeight = m_CmdRunButton.fixedHeight;
            if (!string.IsNullOrEmpty(m_Cmd))
            {
                float gap = m_CmdContentStyle.lineHeight + m_CmdContentStyle.margin.top + m_CmdContentStyle.padding.top;
                cmdHeight = Mathf.Max(cmdHeight, m_Cmd.Split('\n').Length * gap);
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
using UnityEngine;
using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;
using XFrame.Core;

namespace UnityXFrame.Core.UIElements
{
    /// <summary>
    /// UI基类
    /// </summary>
    public abstract partial class UI : Container, IUI
    {
        private bool m_Active;

        protected int m_Layer;
        protected bool m_IsOpen;
        protected IUIGroup m_Group;
        protected internal GameObject m_Root;
        protected RectTransform m_Transform;
        protected CanvasGroup m_CanvasGroup;
        protected UIFinder m_UIFinder;

        #region UI Interface
        public int Layer
        {
            get { return m_Layer; }
            set { m_Layer = UIModule.SetLayer(m_Transform.parent, this, value); }
        }

        public bool Active
        {
            get => m_Active;
            set
            {
                if (value)
                {
                    m_Active = true;
                    m_CanvasGroup.alpha = 1;
                    m_CanvasGroup.blocksRaycasts = true;
                }
                else
                {
                    m_Active = false;
                    m_CanvasGroup.alpha = 0;
                    m_CanvasGroup.blocksRaycasts = false;
                }
            }
        }

        public bool IsOpen => m_IsOpen;

        IUIGroup IUI.Group
        {
            get { return m_Group; }
            set { m_Group = value; }
        }

        public IEventSystem Event { get; private set; }

        public RectTransform Root => m_Transform;

        public string Name => m_Root.name;

        public void Open()
        {
            if (m_IsOpen)
                return;
            m_IsOpen = true;

            if (m_Group != null)
                m_Group.OpenUI(this);
            else
                Log.Error(nameof(UIModule), "UI Group is null.");
        }

        public void Close()
        {
            if (!m_IsOpen)
                return;
            m_IsOpen = false;

            if (m_Group != null)
                m_Group.CloseUI(this);
            else
                Log.Error(nameof(UIModule), "UI Group is null.");
        }

        void IUI.OnOpen()
        {
            foreach (ICom com in this)
            {
                UICom uiCom = com as UICom;
                if (uiCom != null)
                    uiCom.OnOpen();
            }
            OnOpen();
        }

        void IUI.OnClose()
        {
            foreach (ICom com in this)
            {
                UICom uiCom = com as UICom;
                if (uiCom != null)
                    uiCom.OnClose();
            }

            OnClose();
        }
        #endregion
        protected override void OnInit()
        {
            base.OnInit();
            m_UIFinder = GetOrAddCom<UIFinder>();
        }

        protected override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            m_CanvasGroup = m_Root.GetComponent<CanvasGroup>();
            if (m_CanvasGroup == null)
                m_CanvasGroup = m_Root.AddComponent<CanvasGroup>();
            m_Transform = m_Root.GetComponent<RectTransform>();
            Event = XModule.Event.NewSys();
            m_IsOpen = false;
            Active = false;
        }

        #region Sub Class Implement Life Fun
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        #endregion
    }
}

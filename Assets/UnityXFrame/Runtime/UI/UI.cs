using UnityEngine;
using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI基类
    /// </summary>
    public abstract partial class UI : Container, IUI
    {
        protected int m_Layer;
        protected bool m_IsOpen;
        protected IUIGroup m_Group;
        protected internal GameObject m_Root;
        protected RectTransform m_Transform;
        protected UIFinder m_UIFinder;

        #region UI Interface
        public int Layer
        {
            get { return m_Layer; }
            set { m_Layer = UIModule.SetLayer(m_Transform.parent, this, value); }
        }

        public bool Active
        {
            get => m_Root.activeSelf;
            set => m_Root.SetActive(value);
        }

        public bool IsOpen => m_IsOpen;

        IUIGroup IUI.Group
        {
            get { return m_Group; }
            set { m_Group = value; }
        }

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

            IUIGroup group = m_Group;
            if (m_Group != null)
                group.CloseUI(this);
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
            m_Transform = m_Root.GetComponent<RectTransform>();
            m_IsOpen = false;
            Active = false;
        }

        #region Sub Class Implement Life Fun
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        #endregion
    }
}

using UnityEngine;
using XFrame.Collections;
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
        protected GameObject m_Root;
        protected RectTransform m_Transform;

        #region UI Interface
        int IUI.Layer
        {
            get { return m_Layer; }
            set { ((IUI)this).SetLayer(value, true); }
        }

        bool IUI.Active
        {
            get => m_Root.activeSelf;
            set => m_Root.SetActive(value);
        }

        bool IUI.IsOpen => m_IsOpen;

        IUIGroup IUI.Group => m_Group;

        public RectTransform Root => m_Transform;

        public string Name => m_Root.name;

        public void Open()
        {
            if (m_IsOpen)
                return;
            m_IsOpen = true;
            IUIGroup group = m_Group;
            if (group != null)
            {
                group.OpenUI(this);
            }
            else
            {
                Log.Error(nameof(UIModule), "UI Group is null.");
            }
        }

        public void Close()
        {
            if (!m_IsOpen)
                return;
            m_IsOpen = false;
            IUIGroup group = m_Group;
            if (group != null)
            {
                group.CloseUI(this);
            }
            else
            {
                Log.Error(nameof(UIModule), "UI Group is null.");
            }
        }

        void IUI.OnGroupChange(IUIGroup newGroup)
        {
            m_Group = newGroup as UIGroup;
        }

        void IUI.OnInit(int id, OnUIReady onReady)
        {
            IContainer thisContainer = this;
            thisContainer.OnInit(id, this, (c) =>
            {
                onReady?.Invoke(this);
                m_Root = c.GetData<GameObject>();
                m_Transform = m_Root.GetComponent<RectTransform>();
            });
        }

        void IContainer.OnUpdate(float elapseTime)
        {
            if (m_IsOpen)
            {
                SetIt(XItType.Forward);
                foreach (ICom com in this)
                {
                    if (com.Active)
                        com.OnUpdate(elapseTime);
                }
                OnUpdate(elapseTime);
            }
        }

        void IContainer.OnDestroy()
        {
            SetIt(XItType.Backward);
            foreach (ICom com in this)
                com.OnDestroy();
            OnDestroy();
            GameObject.Destroy(m_Root);
            m_Root = null;
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

        void IUI.SetLayer(int layer, bool refresh)
        {
            m_Layer = layer;
            if (refresh)
                m_Group?.SetUILayer(this, m_Layer);
        }
        #endregion

        #region Sub Class Implement Life Fun
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        #endregion
    }
}

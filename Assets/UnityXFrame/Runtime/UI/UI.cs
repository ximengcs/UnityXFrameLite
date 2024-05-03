using UnityEngine;
using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.UIElements
{
    /// <summary>
    /// UI基类
    /// </summary>
    public abstract partial class UI : Container, IUI, ICanUpdateLayerValue
    {
        private int m_Layer;

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
            set
            {
                UIGroup uiGroup = m_Group as UIGroup;
                m_Layer = UIModule.SetLayer(m_Transform.parent, this, value, uiGroup.InnerUIIndexChange);
            }
        }

        public override void SetActive(bool active, bool recursive = true)
        {
            if (active)
            {
                m_IsActive = true;
                m_CanvasGroup.alpha = 1;
                m_CanvasGroup.blocksRaycasts = true;
            }
            else
            {
                m_IsActive = false;
                m_CanvasGroup.alpha = 0;
                m_CanvasGroup.blocksRaycasts = false;
            }
            base.SetActive(active, recursive);
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

        void ICanUpdateLayerValue.SetLayerValue(int layer)
        {
            m_Layer = layer;
        }

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

        void IPoolObject.OnCreate()
        {
            IPoolObject poolObj = this;
            m_Module = poolObj.InPool.Module.Domain.GetModule<IContainerModule>();
            m_CanvasGroup = m_Root.GetComponent<CanvasGroup>();
            if (m_CanvasGroup == null)
                m_CanvasGroup = m_Root.AddComponent<CanvasGroup>();
            m_Transform = m_Root.GetComponent<RectTransform>();
            Event = m_Module.Domain.GetModule<IEventModule>().NewSys();
            m_IsOpen = false;
            SetActive(false, false);
            OnCreateFromPool();
        }

        IPool IPoolObject.InPool { get; set; }
        int IPoolObject.PoolKey { get; }
        string IPoolObject.MarkName { get; set; }

        void IPoolObject.OnRequest() { OnRequestFromPool(); }
        void IPoolObject.OnRelease() { OnReleaseFromPool(); }
        void IPoolObject.OnDelete() { OnDestroyFromPool(); }

        #region Sub Class Implement Life Fun
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        protected virtual void OnCreateFromPool() { }
        protected virtual void OnRequestFromPool() { }
        protected virtual void OnDestroyFromPool() { }
        protected virtual void OnReleaseFromPool() { }
        #endregion
    }
}

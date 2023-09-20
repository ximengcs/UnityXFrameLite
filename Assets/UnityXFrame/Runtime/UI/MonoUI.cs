using System;
using UnityEngine;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIElements
{
    [DefaultExecutionOrder(Constant.EXECORDER_AFTER)]
    public abstract partial class MonoUI : MonoBehaviour, IUI
    {
        private bool m_Active;
        protected bool m_IsOpen;
        protected int m_Layer;
        protected IUIGroup m_Group;
        protected GameObject m_Root;
        protected CanvasGroup m_CanvasGroup;
        protected RectTransform m_Transform;
        protected UIFinder m_UIFinder;

        private IContainer m_Container;
        private MonoContainer m_MonoContainer;

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

        public int Id => m_Container.Id;

        public RectTransform Root => m_Transform;

        public string Name => m_Root.name;

        public IContainer Master => m_Container.Master;

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

        void IContainer.OnInit(int id, IContainer master, OnDataProviderReady onReady)
        {
            m_Container.OnInit(id, this, null);
            onReady?.Invoke(this);
            m_UIFinder = GetOrAddCom<UIFinder>();
            OnInit();
        }

        void IContainer.OnUpdate(float elapseTime)
        {
            m_Container.OnUpdate(elapseTime);
            OnUpdate(elapseTime);
        }

        void IContainer.OnDestroy()
        {
            m_Container.OnDestroy();
            OnUIDestroy();
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

        int IPoolObject.PoolKey => 0;

        IPool IPoolObject.InPool { get; set; }

        void IPoolObject.OnCreate()
        {
            m_MonoContainer = new MonoContainer();
            m_Container = m_MonoContainer;
            m_MonoContainer.TriggerOnCreateFromPool();

            m_Root = gameObject;
            m_CanvasGroup = m_Root.GetComponent<CanvasGroup>();
            if (m_CanvasGroup == null)
                m_CanvasGroup = m_Root.AddComponent<CanvasGroup>();
            m_Transform = m_Root.GetComponent<RectTransform>();
            Event = EventModule.Inst.NewSys();
            m_IsOpen = false;
            Active = false;
            OnCreateFromPool();
        }

        void IPoolObject.OnRequest()
        {
            m_MonoContainer.TriggerOnRequestFromPool();
            OnRequestFromPool();
        }

        void IPoolObject.OnRelease()
        {
            OnReleaseFromPool();
            m_MonoContainer.TriggerOnReleaseFromPool();
        }

        void IPoolObject.OnDelete()
        {
            OnDestroyFromPool();
            GameObject.Destroy(m_Root);
            m_MonoContainer.TriggerOnDestroyFromPool();
        }

        protected virtual void OnInit() { }
        protected virtual void OnUpdate(float elapseTime) { }
        protected virtual void OnUIDestroy() { }
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        protected virtual void OnCreateFromPool() { }
        protected virtual void OnRequestFromPool() { }
        protected virtual void OnDestroyFromPool() { }
        protected virtual void OnReleaseFromPool() { }

        public T GetCom<T>(int id = 0) where T : ICom
        {
            return m_Container.GetCom<T>(id);
        }

        public ICom GetCom(Type type, int id = 0)
        {
            return m_Container.GetCom(type, id);
        }

        public T AddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Container.AddCom<T>(onReady);
        }

        public ICom AddCom(ICom com)
        {
            return m_Container.AddCom(com);
        }

        public T AddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Container.AddCom<T>(id, onReady);
        }

        public ICom AddCom(Type type, OnDataProviderReady onReady = null)
        {
            return m_Container.AddCom(type, onReady);
        }

        public ICom AddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return m_Container.AddCom(type, id, onReady);
        }

        public T GetOrAddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Container.GetOrAddCom<T>(onReady);
        }

        public T GetOrAddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Container.GetOrAddCom<T>(id, onReady);
        }

        public ICom GetOrAddCom(Type type, OnDataProviderReady onReady = null)
        {
            return m_Container.GetOrAddCom(type, onReady);
        }

        public ICom GetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return m_Container.GetOrAddCom(type, id, onReady);
        }

        public void RemoveCom<T>(int id = 0) where T : ICom
        {
            m_Container.RemoveCom<T>(id);
        }

        public void RemoveCom(Type type, int id = 0)
        {
            m_Container.RemoveCom(type, id);
        }

        public void ClearCom()
        {
            m_Container.ClearCom();
        }

        public void SetData<T>(T value)
        {
            m_Container.SetData(value);
        }

        public T GetData<T>()
        {
            return m_Container.GetData<T>();
        }

        public void SetData<T>(string name, T value)
        {
            m_Container.SetData(name, value);
        }

        public T GetData<T>(string name)
        {
            return m_Container.GetData<T>(name);
        }

        public void ClearData()
        {
            m_Container.ClearData();
        }

        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Container.GetEnumerator();
        }

        public void SetIt(XItType type)
        {
            m_Container.SetIt(type);
        }
    }
}

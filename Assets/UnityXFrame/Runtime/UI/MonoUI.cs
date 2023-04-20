using System;
using UnityEngine;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIs
{
    public abstract partial class MonoUI : MonoBehaviour, IUI
    {
        private IContainer m_Container;

        protected bool m_IsOpen;
        protected int Layer;
        protected IUIGroup m_Group;
        protected GameObject m_Root;
        protected RectTransform m_Transform;

        int IUI.Layer
        {
            get { return Layer; }
            set
            {
                Layer = value;
                m_Group?.SetUILayer(this, Layer);
            }
        }

        bool IUI.Active
        {
            get => m_Root.activeSelf;
            set => m_Root.SetActive(value);
        }

        bool IUI.IsOpen => m_IsOpen;

        IUIGroup IUI.Group => m_Group;

        public int Id => m_Container.Id;

        public RectTransform Root => m_Transform;

        public string Name => m_Root.name;

        public object Master => m_Container.Master;

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
                m_Root = GetData<GameObject>();
                m_Transform = m_Root.GetComponent<RectTransform>();
            });
        }

        void IContainer.OnInit(int id, object master, OnContainerReady onReady)
        {
            m_Container = new Container();
            m_Container.OnInit(id, master, null);
            onReady?.Invoke(this);
            OnInit();
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
            OnUIDestroy();
            GameObject.Destroy(m_Root);
            m_Root = null;
        }

        void IUI.OnOpen()
        {
            OnOpen();
        }

        void IUI.OnClose()
        {
            OnClose();
        }

        void IUI.SetLayer(int layer, bool refresh)
        {
            Layer = layer;
            if (refresh)
                m_Group?.SetUILayer(this, Layer);
        }

        void IPoolObject.OnCreate()
        {
            OnCreateFromPool();
        }

        void IPoolObject.OnRelease()
        {
            OnReleaseFromPool();
        }

        void IPoolObject.OnDelete()
        {
            OnDestroyFromPool();
        }

        protected virtual void OnInit() { }
        protected virtual void OnUpdate(float elapseTime) { }
        protected virtual void OnUIDestroy() { }
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        protected virtual void OnCreateFromPool() { }
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

        public T AddCom<T>(OnComReady onReady = null) where T : ICom
        {
            return m_Container.AddCom<T>(onReady);
        }

        public ICom AddCom(ICom com, int id = 0, OnComReady onReady = null)
        {
            return m_Container.AddCom(com, id, onReady);
        }

        public T AddCom<T>(int id, OnComReady onReady = null) where T : ICom
        {
            return m_Container.AddCom<T>(id, onReady);
        }

        public ICom AddCom(Type type, OnComReady onReady = null)
        {
            return m_Container.AddCom(type, onReady);
        }

        public ICom AddCom(Type type, int id = 0, OnComReady onReady = null)
        {
            return m_Container.AddCom(type, id, onReady);
        }

        public T GetOrAddCom<T>(OnComReady onReady = null) where T : ICom
        {
            return m_Container.GetOrAddCom<T>(onReady);
        }

        public T GetOrAddCom<T>(int id = 0, OnComReady onReady = null) where T : ICom
        {
            return m_Container.GetOrAddCom<T>(id, onReady);
        }

        public ICom GetOrAddCom(Type type, OnComReady onReady = null)
        {
            return m_Container.GetOrAddCom(type, onReady);
        }

        public ICom GetOrAddCom(Type type, int id = 0, OnComReady onReady = null)
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

        public void DispatchCom(OnComReady handle)
        {
            m_Container.DispatchCom(handle);
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

        public void Dispose()
        {
            m_Container.Dispose();
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

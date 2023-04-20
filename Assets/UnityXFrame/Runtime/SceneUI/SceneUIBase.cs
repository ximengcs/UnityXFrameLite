using UnityEngine;
using XFrame.Modules.Entities;
using System.Collections.Generic;

namespace UnityXFrame.Core.SceneUIs
{
    public abstract class SceneUI : ISceneUI
    {
        private List<ISceneUI> m_SubItems;

        protected RectTransform m_Root;
        protected Entity m_Entity;
        protected bool m_IsOpen;
        protected bool m_AnyPosClose;

        public virtual void OnInit(Entity entity, RectTransform root)
        {
            m_SubItems = new List<ISceneUI>();
            m_Entity = entity;
            m_Root = root;
            m_Root.gameObject.SetActive(false);
        }

        protected T AddSubItem<T>(int index, RectTransform root) where T : SceneUIGroupItem, new()
        {
            T item = new T();
            item.OnInit(index, root, m_Entity);
            m_SubItems.Add(item);
            return item;
        }

        public void Open()
        {
            if (m_IsOpen)
                return;
            m_IsOpen = true;
            m_Root.gameObject.SetActive(true);
            SceneUIModule.Inst.UpdateUIState(this);

            OnOpen();
        }

        protected virtual void OnOpen()
        {

        }

        protected virtual void OnClose()
        {

        }

        public void Close()
        {
            if (!m_IsOpen)
                return;
            m_IsOpen = false;
            m_Root.gameObject.SetActive(false);

            OnClose();
        }

        public virtual void OnUpdate()
        {
            if (m_AnyPosClose)
            {
                if (Input.GetMouseButtonDown(0))
                    Close();
            }

            foreach (ISceneUI ui in m_SubItems)
                ui.OnUpdate();
        }

        public virtual void OnDestroy()
        {
            foreach (ISceneUI ui in m_SubItems)
                ui.OnDestroy();
            m_SubItems = null;
        }
    }
}

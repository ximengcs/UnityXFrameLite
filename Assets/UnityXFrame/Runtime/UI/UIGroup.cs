using System;
using UnityEngine;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.UIs
{
    public partial class UIGroup : IUIGroup
    {
        private int m_Layer;
        private bool m_Active;
        private RectTransform m_Root;
        private GameObject m_Inst;
        private XLinkList<IUI> m_UIs;
        private CanvasGroup m_CanvasGroup;
        private XLinkList<IUIGroupHelper> m_UIHelper;

        public RectTransform Root => m_Root;

        public IEventSystem Event { get; private set; }

        public string Name { get; }

        public bool IsOpen { get; private set; }

        public float Alpha
        {
            get => m_CanvasGroup.alpha;
            set => m_CanvasGroup.alpha = value;
        }

        public int Layer
        {
            get { return m_Layer; }
            set
            {
                m_Layer = UIModule.Inst.SetUIGroupLayer(this, value);
            }
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

        public UIGroup(GameObject root, string name, int layer)
        {
            Name = name;
            Layer = layer;
            m_Inst = root;
            m_Root = (RectTransform)root.transform;
            m_UIs = new XLinkList<IUI>();
            m_UIHelper = new XLinkList<IUIGroupHelper>();
            m_CanvasGroup = root.GetComponent<CanvasGroup>();
            Event = EventModule.Inst.NewSys();

            m_Root.anchorMin = Vector3.zero;
            m_Root.anchorMax = Vector3.one;
            m_Root.offsetMin = Vector2.zero;
            m_Root.offsetMax = Vector2.zero;
            m_Root.anchoredPosition3D = Vector3.zero;
        }

        public void Open()
        {
            if (IsOpen)
                return;
            IsOpen = true;
            Active = true;
        }

        public void Close()
        {
            if (!IsOpen)
                return;
            IsOpen = false;
            Active = false;
        }

        void IUIGroup.CloseUI(IUI ui)
        {
            InnerTriggerEvent(ui, UICloseBeforeEvent.Create(ui));
            if (m_UIHelper != null && m_UIHelper.Count > 0)
            {
                foreach (XLinkNode<IUIGroupHelper> helperNode in m_UIHelper)
                {
                    IUIGroupHelper helper = helperNode.Value;
                    if (helper.MatchType(ui.GetType()))
                    {
                        helper.OnUIClose(ui);
                        break;
                    }
                }
            }
            else
            {
                ui.OnClose();
                ui.Active = false;
                InnerTriggerEvent(ui, UICloseEvent.Create(ui));
            }
        }

        internal void InnerTriggerEvent(IUI ui, UIEvent e)
        {
            UIEvent clone = e.Clone();
            ui.Event.TriggerNow(e);
            Event.Trigger(clone);
            UIModule.Inst.Event.Trigger(clone.Clone());
        }

        void IUIGroup.OpenUI(IUI ui)
        {
            InnerTriggerEvent(ui, UIOpenBeforeEvent.Create(ui));
            if (m_UIHelper != null && m_UIHelper.Count > 0)
            {
                foreach (XLinkNode<IUIGroupHelper> helperNode in m_UIHelper)
                {
                    IUIGroupHelper helper = helperNode.Value;
                    if (helper.MatchType(ui.GetType()))
                    {
                        helper.OnUIOpen(ui);
                        break;
                    }
                }
            }
            else
            {
                ui.OnOpen();
                ui.Active = true;
                InnerTriggerEvent(ui, UIOpenEvent.Create(ui));
            }
        }

        void IUIGroup.OnInit()
        {
            Open();
        }

        void IUIGroup.OnUpdate(float elapseTime)
        {
            if (IsOpen)
            {
                foreach (XLinkNode<IUIGroupHelper> helperNode in m_UIHelper)
                {
                    IUIGroupHelper helper = helperNode.Value;
                    helper.OnUpdate();

                    foreach (XLinkNode<IUI> node in m_UIs)
                    {
                        if (!node.Value.IsOpen)
                            continue;
                        if (m_UIHelper != null && m_UIHelper.Count > 0)
                        {
                            if (helper.MatchType(node.Value.GetType()))
                            {
                                helper.OnUIUpdate(node.Value, elapseTime);
                                break;
                            }
                        }
                        else
                        {
                            node.Value.OnUpdate(elapseTime);
                        }
                    }
                }
            }
        }

        void IUIGroup.OnDestroy()
        {
            foreach (XLinkNode<IUI> node in m_UIs)
            {
                IUI ui = node.Value;
                if (m_UIHelper != null && m_UIHelper.Count > 0)
                {
                    foreach (XLinkNode<IUIGroupHelper> helperNode in m_UIHelper)
                    {
                        IUIGroupHelper helper = helperNode.Value;
                        if (helper.MatchType(ui.GetType()))
                        {
                            helper.OnUIDestroy(ui);
                            break;
                        }
                        helper.OnDestroy();
                    }
                }
                else
                    ui.OnDestroy();
            }
        }

        void IUIGroup.AddUI(IUI ui)
        {
            InnerRemoveOldGroup(ui);
            ui.Root.SetParent(m_Root, false);
            ui.Layer = m_UIs.Count;
            ui.Group = this;
            m_UIs.AddLast(ui);
        }

        void IUIGroup.RemoveUI(IUI ui)
        {
            InnerRemoveOldGroup(ui);
            ui.Root.SetParent(null, false);
        }

        private void InnerRemoveOldGroup(IUI ui)
        {
            if (ui.Group != null)
            {
                bool remove = false;
                UIGroup old = (UIGroup)ui.Group;
                foreach (XLinkNode<IUI> node in old.m_UIs)
                {
                    if (!remove)
                    {
                        if (node.Value == ui)
                        {
                            node.Delete();
                            remove = true;
                        }
                    }
                    else
                    {
                        node.Value.Layer--;
                    }
                }
            }
        }

        public IUIGroupHelper AddHelper(Type type)
        {
            IUIGroupHelper helper = (IUIGroupHelper)Activator.CreateInstance(type);
            InnerAddHelper(helper);
            return helper;
        }

        public IUIGroupHelper AddHelper(IUIGroupHelper helper)
        {
            InnerAddHelper(helper);
            return helper;
        }

        public T AddHelper<T>() where T : IUIGroupHelper
        {
            return (T)AddHelper(typeof(T));
        }

        public void RemoveHelper<T>() where T : IUIGroupHelper
        {
            RemoveHelper(typeof(T));
        }

        public void RemoveHelper(Type type)
        {
            InnerRemoveHelper(type);
        }

        private void InnerAddHelper(IUIGroupHelper helper)
        {
            m_UIHelper.AddLast(helper);
            helper.OnInit(this);
        }

        private void InnerRemoveHelper(Type type)
        {
            XLinkNode<IUIGroupHelper> node = m_UIHelper.First;
            while (node != null)
            {
                if (node.Value.GetType() == type)
                {
                    node.Value.OnDestroy();
                    node.Delete();
                    break;
                }
                node = node.Next;
            }
        }

        public IEnumerator<IUI> GetEnumerator()
        {
            return new Enumerator(m_UIs);
        }

        public void SetIt(XItType type)
        {
            m_UIs.SetIt(type);
        }
    }
}

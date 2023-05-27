using System;
using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    public abstract class UIGroupHelperBase : IUIGroupHelper
    {
        protected IEventSystem m_EvtSys;
        protected IUIGroup m_Owner;

        bool IUIGroupHelper.MatchType(Type type)
        {
            return MatchType(type);
        }

        void IUIGroupHelper.OnInit(IUIGroup owner)
        {
            m_Owner = owner;
            OnInit();
        }

        void IUIGroupHelper.OnUpdate()
        {
            OnUpdate();
        }

        void IUIGroupHelper.OnDestroy()
        {
            OnDestroy();
        }

        void IUIGroupHelper.OnUIClose(IUI ui)
        {
            OnUIClose(ui);
        }

        void IUIGroupHelper.OnUIDestroy(IUI ui)
        {
            OnUIDestroy(ui);
        }

        void IUIGroupHelper.OnUIOpen(IUI ui)
        {
            OnUIOpen(ui);
        }

        void IUIGroupHelper.OnUIUpdate(IUI ui, float elapseTime)
        {
            OnUIUpdate(ui, elapseTime);
        }

        protected virtual bool MatchType(Type type) { return true; }
        protected virtual void OnInit() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnDestroy() { }
        protected virtual void OnUIClose(IUI ui)
        {
            InnerCloseUI(ui);
            ui.Active = false;
        }
        protected virtual void OnUIDestroy(IUI ui)
        {
            InnerDestroyUI(ui);
        }
        protected virtual void OnUIOpen(IUI ui)
        {
            ui.Active = true;
            InnerOpenUI(ui);
        }
        protected virtual void OnUIUpdate(IUI ui, float elapseTime)
        {
            InnerUpdateUI(ui, elapseTime);
        }

        protected void InnerOpenUI(IUI ui)
        {
            ui.OnOpen();
        }

        protected void InnerCloseUI(IUI ui)
        {
            ui.OnClose();
        }

        protected void InnerUpdateUI(IUI ui, float elapseTime)
        {
            ui.OnUpdate(elapseTime);
        }

        protected void InnerDestroyUI(IUI ui)
        {
            ui.OnDestroy();
        }

        protected void InnerSetUIActive(IUI ui, bool value)
        {
            ui.Active = value;
        }
    }
}

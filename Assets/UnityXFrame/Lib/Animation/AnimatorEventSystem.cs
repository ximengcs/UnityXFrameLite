using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Event;

namespace UnityXFrameLib.Animations
{
    [DefaultExecutionOrder(Constant.EXECORDER_AFTER)]
    public class AnimatorEventSystem : MonoBehaviour, IEventSystem
    {
        private IEventSystem m_Event;
        private IEventSystem Event
        {
            get
            {
                if (m_Event == null)
                    m_Event = EventModule.Inst.NewSys();
                return m_Event;
            }
        }

        public void Trigger(AnimationEvent e)
        {
            Event.Trigger(e.stringParameter.GetHashCode());
        }

        public void Listen(string name, XEventHandler handler)
        {
            Event.Listen(name.GetHashCode(), handler);
        }

        public void Listen(string name, XEventHandler2 handler)
        {
            Event.Listen(name.GetHashCode(), handler);
        }

        public void UnListen(string name, XEventHandler handler)
        {
            Event.Listen(name.GetHashCode(), handler);
        }

        public void UnListen(string name, XEventHandler2 handler)
        {
            Event.Listen(name.GetHashCode(), handler);
        }

        public void Trigger(int eventId)
        {
            Event.Trigger(eventId);
        }

        public void Trigger(XEvent e)
        {
            Event.Trigger(e);
        }

        public void TriggerNow(int eventId)
        {
            Event.TriggerNow(eventId);
        }

        public void TriggerNow(XEvent e)
        {
            Event.TriggerNow(e);
        }

        public void Listen(int eventId, XEventHandler handler)
        {
            Event.Listen(eventId, handler);
        }

        public void Listen(int eventId, XEventHandler2 handler)
        {
            Event.Listen(eventId, handler);
        }

        public void Unlisten(int eventId, XEventHandler handler)
        {
            Event.Unlisten(eventId, handler);
        }

        public void Unlisten(int eventId, XEventHandler2 handler)
        {
            Event.Unlisten(eventId, handler);
        }

        public void Unlisten(int eventId)
        {
            Event.Unlisten(eventId);
        }

        public void Unlisten()
        {
            Event.Unlisten();
        }
    }
}

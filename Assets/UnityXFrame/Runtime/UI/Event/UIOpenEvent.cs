using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    public class UIOpenEvent : XEvent
    {
        private static int m_EventId;

        public static int EventId
        {
            get
            {
                if (m_EventId == 0)
                    m_EventId = typeof(UIOpenEvent).GetHashCode();
                return m_EventId;
            }
        }

        public IUI Target { get; internal set; }

        public UIOpenEvent() : base(EventId) { }
    }
}

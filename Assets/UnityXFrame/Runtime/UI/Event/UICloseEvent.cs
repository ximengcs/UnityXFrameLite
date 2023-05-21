using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    public class UICloseEvent : XEvent
    {
        private static int m_EventId;

        public static int EventId
        {
            get
            {
                if (m_EventId == 0)
                    m_EventId = typeof(UICloseEvent).GetHashCode();
                return m_EventId;
            }
        }

        public IUI Target { get; internal set; }

        public UICloseEvent() : base(EventId) { }
    }
}

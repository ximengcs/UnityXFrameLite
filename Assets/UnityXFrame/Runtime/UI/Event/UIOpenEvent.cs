using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    public class UIOpenEvent : UIEvent
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

        public UIOpenEvent() : this(default) { }

        public UIOpenEvent(IUI ui) : base(ui, EventId) { }
    }
}

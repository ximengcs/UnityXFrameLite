using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    public class UICloseEvent : UIEvent
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

        public UICloseEvent() : this(default) { }

        public UICloseEvent(IUI ui) : base(ui, EventId) { }
    }
}

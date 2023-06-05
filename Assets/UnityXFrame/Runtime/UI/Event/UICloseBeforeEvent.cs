
namespace UnityXFrame.Core.UIs
{
    public class UICloseBeforeEvent : UIEvent
    {
        private static int m_EventId;

        public static int EventId
        {
            get
            {
                if (m_EventId == 0)
                    m_EventId = typeof(UICloseBeforeEvent).GetHashCode();
                return m_EventId;
            }
        }

        public UICloseBeforeEvent() : this(default) { }

        public UICloseBeforeEvent(IUI ui) : base(ui, EventId) { }
    }
}

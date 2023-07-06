using XFrame.Modules.Pools;

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

        public static UICloseEvent Create(IUI ui)
        {
            UICloseEvent evt = References.Require<UICloseEvent>();
            evt.Id = EventId;
            evt.Target = ui;
            return evt;
        }
    }
}

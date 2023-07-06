using XFrame.Modules.Event;
using XFrame.Modules.Pools;

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

        public static UIOpenEvent Create(IUI ui)
        {
            UIOpenEvent evt = References.Require<UIOpenEvent>();
            evt.Id = EventId;
            evt.Target = ui;
            return evt;
        }
    }
}

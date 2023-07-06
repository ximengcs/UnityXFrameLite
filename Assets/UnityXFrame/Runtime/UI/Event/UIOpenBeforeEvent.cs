
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.UIs
{
    public class UIOpenBeforeEvent : UIEvent
    {
        private static int m_EventId;

        public static int EventId
        {
            get
            {
                if (m_EventId == 0)
                    m_EventId = typeof(UIOpenBeforeEvent).GetHashCode();
                return m_EventId;
            }
        }

        public static UIOpenBeforeEvent Create(IUI ui)
        {
            UIOpenBeforeEvent evt = References.Require<UIOpenBeforeEvent>();
            evt.Id = EventId;
            evt.Target = ui;
            return evt;
        }
    }
}


using XFrame.Modules.Pools;

namespace UnityXFrame.Core.UIElements
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

        public static UICloseBeforeEvent Create(IUI ui)
        {
            UICloseBeforeEvent evt = References.Require<UICloseBeforeEvent>();
            evt.Id = EventId;
            evt.Target = ui;
            return evt;
        }
    }
}


using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    public class UIGroupChangeEvent : XEvent
    {
        public static int EventId => typeof(UIGroupChangeEvent).GetHashCode();

        public UIGroupChangeEvent() : base(EventId) { }
    }
}

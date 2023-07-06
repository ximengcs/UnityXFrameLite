using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    public abstract class UIEvent : XEvent
    {
        public IUI Target { get; protected set; }
    }
}

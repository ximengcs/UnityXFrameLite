using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    public abstract class UIEvent : XEvent
    {
        public IUI Target { get; internal set; }

        public UIEvent(IUI ui, int eventId) : base(eventId)
        {
            Target = ui;
        }
    }
}

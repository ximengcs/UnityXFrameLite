using UnityEngine;
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.UIElements
{
    public abstract class UIEvent : XEvent
    {
        public IUI Target { get; protected set; }

        public UIEvent Clone()
        {
            UIEvent e = (UIEvent)References.Require(GetType());
            e.Target = Target;
            e.Id = Id;
            return e;
        }
    }
}

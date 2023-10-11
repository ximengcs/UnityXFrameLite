using UnityEngine;
using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIElements
{
    public partial class UIModule
    {
        public struct Data
        {
            public IEventSystem Event;
            public Canvas Canvas;

            public Data(IEventSystem e, Canvas canvas)
            {
                Event = e;
                Canvas = canvas;
            }
        }
    }
}

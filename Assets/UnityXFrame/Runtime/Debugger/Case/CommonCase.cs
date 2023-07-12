
using UnityEngine;
using UnityEngine.EventSystems;
using UnityXFrame.Core.UIs;
using XFrame.Modules.Event;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugWindow(-996)]
    public class CommonCase : IDebugWindow
    {
        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        private IEventSystem Sys;
        public void OnDraw()
        {
            if (DebugGUI.Button("init event"))
            {
                Sys = EventModule.Inst.NewSys();
            }
            if (DebugGUI.Button("Listen UI"))
            {
                UIModule.Inst.Event.Listen(UIOpenEvent.EventId, (e) => Debug.Log("test"));
            }
            if (DebugGUI.Button("Trigger"))
            {
                UIModule.Inst.Event.Trigger(UIOpenEvent.EventId);
            }
        }
    }
}
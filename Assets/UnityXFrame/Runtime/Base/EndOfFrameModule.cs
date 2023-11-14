using System;
using System.Collections.Generic;
using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Tasks;

namespace UnityXFrame.Core
{
    [CommonModule]
    public partial class EndOfFrameModule : ModuleBase, IEndOfFrame
    {
        private List<TaskHandler> m_Handlers;
        private WaitForEndOfFrame m_WaitYield;

        internal bool Empty => m_Handlers.Count == 0;
        internal WaitForEndOfFrame WaitYield => m_WaitYield;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_WaitYield = new WaitForEndOfFrame();
            m_Handlers = new List<TaskHandler>(16);
        }

        public ITaskHandler Request(Action action)
        {
            TaskHandler handler = new TaskHandler(action);
            m_Handlers.Add(handler);
            return handler;
        }

        public void OnEndOfFrame()
        {
            for (int i = m_Handlers.Count - 1; i >= 0; i--)
            {
                TaskHandler handler = m_Handlers[i];
                if (handler.Start)
                {
                    handler.Complete = true;
                    m_Handlers.RemoveAt(i);
                }
            }
        }
    }
}

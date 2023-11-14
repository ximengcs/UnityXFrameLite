using System;
using XFrame.Modules.Tasks;

namespace UnityXFrame.Core
{
    public partial class EndOfFrameModule
    {
        public class TaskHandler : ITaskHandler
        {
            private Action m_Action;
            private bool m_Complete;

            internal bool Complete
            {
                get => m_Complete;
                set
                {
                    if (m_Complete != value)
                    {
                        m_Complete = value;
                        if (m_Complete)
                        {
                            m_Action();
                            m_Action = null;
                        }
                    }
                }
            }

            internal bool Start { get; set; }

            internal TaskHandler(Action action)
            {
                m_Action = action;
                Complete = false;
                Start = false;
            }
        }

        public class TaskStrategy : ITaskStrategy<TaskHandler>
        {
            private TaskHandler m_Handler;

            public void OnUse(TaskHandler handler)
            {
                m_Handler = handler;
                m_Handler.Start = true;
            }

            public float OnHandle(ITask from)
            {
                return m_Handler.Complete ? TaskBase.MAX_PRO : 0;
            }

            public void OnFinish()
            {
                m_Handler = null;
            }
        }
    }
}

using XFrame.Core;
using XFrame.Core.Threads;
using XFrame.Tasks;

namespace UnityXFrame.Core
{
    public class EndOfFrameXTask : XProTask, ITask
    {
        public EndOfFrameXTask() : base(null)
        {
            m_ProHandler = XTaskHelper.Domain.GetModule<EndOfFrameModule>().InnerRequestHandler(this);
        }
    }

    public partial class EndOfFrameModule
    {
        private class TaskHandler : IProTaskHandler
        {
            private bool m_Complete;
            private IFiberUpdate m_Task;

            public object Data => null;

            public bool IsDone
            {
                get => m_Complete;
                set
                {
                    if (m_Complete != value)
                    {
                        m_Complete = value;
                        if (m_Complete)
                        {
                            m_Task.OnUpdate(0);
                        }
                    }
                }
            }

            public double Pro => m_Complete ? XTaskHelper.MAX_PROGRESS : XTaskHelper.MIN_PROGRESS;

            public TaskHandler(EndOfFrameXTask task)
            {
                IsDone = false;
                m_Task = task;
            }

            public void OnCancel()
            {

            }
        }
    }
}

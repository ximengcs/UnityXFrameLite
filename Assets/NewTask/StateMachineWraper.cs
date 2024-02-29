using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    internal partial class StateMachineWraper<T> where T : IAsyncStateMachine
    {
        private T m_StateMachine;
        private ICancelTask m_Task;
        private ITaskBinder m_Binder;

        private StateMachineWraper()
        {
        }

        private StateMachineWraper(ref T stateMachine, ICancelTask task)
        {
            m_StateMachine = stateMachine;
            m_Task = task;
            m_Binder = task.Binder;
        }

        public void Clear()
        {
            m_StateMachine = default;
            m_Task = null;
            m_Binder = null;
        }
        
        public void Run()
        {
            if (m_Task.Token.Canceled)
            {
                XTaskCancelToken token = m_Task.Token;
                token.Cancel();
                token.InvokeWithoutException();
                XTaskCancelToken.Release(token);
            }
            else if (m_Binder != null && m_Binder.IsDisposed)
            {
                XTaskCancelToken token = m_Task.Token;
                token.Cancel();
                token.InvokeWithoutException();
                XTaskCancelToken.Release(token);
            }
            else
            {
                m_StateMachine.MoveNext();
            }
            
            Release(this);
        }
    }
}
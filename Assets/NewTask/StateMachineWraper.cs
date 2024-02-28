using XFrame.Modules.Diagnotics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace XFrame.Modules.NewTasks
{
    internal partial class StateMachineWraper<T> where T : IAsyncStateMachine
    {
        private T m_StateMachine;
        private ICanelTask m_Task;
        private ITaskBinder m_Binder;

        private StateMachineWraper()
        {
        }

        private StateMachineWraper(ref T stateMachine, ICanelTask task, ITaskBinder binder)
        {
            m_StateMachine = stateMachine;
            m_Task = task;
            m_Binder = binder;
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
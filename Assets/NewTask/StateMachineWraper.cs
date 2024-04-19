using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    internal partial class StateMachineWraper<T> where T : IAsyncStateMachine
    {
        private T m_StateMachine;
        private ICancelTask m_Task;
        private ITaskBinder m_Binder;
        private Action m_SetResult;

        private StateMachineWraper()
        {
        }

        public void Clear()
        {
            m_StateMachine = default;
            m_Task = null;
            m_Binder = null;
        }

        public void RunNoState()
        {
            if (m_Task.Token.Canceled)
            {
                m_Task.SetState(XTaskState.Cancel);
                XTaskCancelToken token = m_Task.Token;
                token.InvokeWithoutException();
                XTaskCancelToken.Release(token);
                m_SetResult();
            }
            else if (m_Binder != null && m_Binder.IsDisposed)
            {
                m_Task.SetState(XTaskState.BinderDispose);
                XTaskCancelToken token = m_Task.Token;
                token.Cancel();
                token.InvokeWithoutException();
                XTaskCancelToken.Release(token);
                m_SetResult();
            }
            else
            {
                m_Task.SetState(XTaskState.Normal);
                m_StateMachine.MoveNext();
            }

            Release(this);
        }

        public void Run(XTaskState awaiterState)
        {
            if (awaiterState != XTaskState.Normal)
            {
                XTaskAction action = XTaskAction.CompleteWhenSubTaskFailure;
                ITask task = m_Task as ITask;
                if (task != null)
                    action = task.TaskAction;

                switch (action)
                {
                    case XTaskAction.CompleteWhenSubTaskFailure:
                        m_Task.SetState(XTaskState.ChildCancel);
                        m_SetResult();
                        break;
                    case XTaskAction.ContinueWhenSubTaskFailure:
                        m_Task.SetState(XTaskState.Normal);
                        m_StateMachine.MoveNext();
                        break;
                }
            }
            else
            {
                if (m_Task.Token.Canceled)
                {
                    m_Task.SetState(XTaskState.Cancel);
                    XTaskCancelToken token = m_Task.Token;
                    token.InvokeWithoutException();
                    XTaskCancelToken.Release(token);
                    m_SetResult();
                }
                else if (m_Binder != null && m_Binder.IsDisposed)
                {
                    m_Task.SetState(XTaskState.BinderDispose);
                    XTaskCancelToken token = m_Task.Token;
                    token.Cancel();
                    token.InvokeWithoutException();
                    XTaskCancelToken.Release(token);
                    m_SetResult();
                }
                else
                {
                    m_Task.SetState(XTaskState.Normal);
                    m_StateMachine.MoveNext();
                }
            }

            Release(this);
        }
    }

    internal partial class StateMachineWraper<T, TResult> where T : IAsyncStateMachine
    {
        private T m_StateMachine;
        private ICancelTask m_Task;
        private ITaskBinder m_Binder;
        private Action<TResult> m_SetResult;

        private StateMachineWraper()
        {
        }

        public void Clear()
        {
            m_StateMachine = default;
            m_Task = null;
            m_Binder = null;
        }


        public void RunNoState()
        {
            if (m_Task.Token.Canceled)
            {
                m_Task.SetState(XTaskState.Cancel);
                XTaskCancelToken token = m_Task.Token;
                token.InvokeWithoutException();
                XTaskCancelToken.Release(token);
                m_SetResult(default);
            }
            else if (m_Binder != null && m_Binder.IsDisposed)
            {
                m_Task.SetState(XTaskState.BinderDispose);
                XTaskCancelToken token = m_Task.Token;
                token.Cancel();
                token.InvokeWithoutException();
                XTaskCancelToken.Release(token);
                m_SetResult(default);
            }
            else
            {
                m_Task.SetState(XTaskState.Normal);
                m_StateMachine.MoveNext();
            }

            Release(this);
        }


        public void Run(XTaskState awaiterState)
        {
            if (awaiterState != XTaskState.Normal)
            {
                XTaskAction action = XTaskAction.CompleteWhenSubTaskFailure;
                ITask task = m_Task as ITask;
                if (task != null)
                    action = task.TaskAction;

                switch (action)
                {
                    case XTaskAction.CompleteWhenSubTaskFailure:
                        m_Task.SetState(XTaskState.ChildCancel);
                        m_SetResult(default);
                        break;
                    case XTaskAction.ContinueWhenSubTaskFailure:
                        m_Task.SetState(XTaskState.Normal);
                        m_StateMachine.MoveNext();
                        break;
                }
            }
            else
            {
                if (m_Task.Token.Canceled)
                {
                    m_Task.SetState(XTaskState.Cancel);
                    XTaskCancelToken token = m_Task.Token;
                    token.InvokeWithoutException();
                    XTaskCancelToken.Release(token);
                }
                else if (m_Binder != null && m_Binder.IsDisposed)
                {
                    m_Task.SetState(XTaskState.BinderDispose);
                    XTaskCancelToken token = m_Task.Token;
                    token.Cancel();
                    token.InvokeWithoutException();
                    XTaskCancelToken.Release(token);
                }
                else
                {
                    m_Task.SetState(XTaskState.Normal);
                    m_StateMachine.MoveNext();
                }
            }

            Release(this);
        }
    }
}
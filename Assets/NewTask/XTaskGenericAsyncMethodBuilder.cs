using System;
using System.Runtime.CompilerServices;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.NewTasks
{
    public struct XTaskAsyncMethodBuilder<T>
    {
        private XTask<T> m_Task;
        private ICancelTask m_CancelTask;

        public XTask<T> Task => m_Task;

        public static XTaskAsyncMethodBuilder<T> Create()
        {
            XTaskAsyncMethodBuilder<T> builder = new XTaskAsyncMethodBuilder<T>();
            builder.m_Task = new XTask<T>();
            builder.m_CancelTask = builder.m_Task;
            return builder;
        }

        public void SetResult(T result)
        {
            m_Task.SetResult(result);
        }


        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            InnerCheckCancel();
            stateMachine.MoveNext();
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            InnerCheckCancel();
            StateMachineWraper<TStateMachine> wraper =
                StateMachineWraper<TStateMachine>.Require(ref stateMachine, m_Task);
            awaiter.OnCompleted(wraper.Run);

            ICancelTask cancelTask = awaiter as ICancelTask;
            if (cancelTask != null)
            {
                cancelTask.Token.AddHandler(stateMachine.MoveNext);
            }
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            InnerCheckCancel();
            StateMachineWraper<TStateMachine> wraper =
                StateMachineWraper<TStateMachine>.Require(ref stateMachine, m_Task);
            awaiter.UnsafeOnCompleted(wraper.Run);

            ICancelTask cancelTask = awaiter as ICancelTask;
            if (cancelTask != null)
            {
                cancelTask.Token.AddHandler(stateMachine.MoveNext);
            }
        }

        public void SetException(Exception e)
        {
            if (e is not OperationCanceledException)
            {
                XTask.ExceptionHandler.Invoke(e);
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        private void InnerCheckCancel()
        {
            if (m_CancelTask.Binder != null)
            {
                if (m_CancelTask.Binder.IsDisposed)
                    m_CancelTask.Token.Cancel();
            }

            m_CancelTask.Token.Invoke();
        }
    }
}
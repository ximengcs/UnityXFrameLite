using System;
using System.Runtime.CompilerServices;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.NewTasks
{
    public struct XTaskAsyncMethodBuilder
    {
        private XTask m_Task;
        private ICanelTask m_CanelTask;

        public XTask Task => m_Task;

        public static XTaskAsyncMethodBuilder Create()
        {
            XTaskAsyncMethodBuilder builder = new XTaskAsyncMethodBuilder();
            builder.m_Task = new XTask();
            builder.m_CanelTask = builder.m_Task;
            return builder;
        }

        public void Coroutine()
        {
        }

        public void SetResult()
        {
            m_Task.SetResult();
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
                StateMachineWraper<TStateMachine>.Require(ref stateMachine, m_Task, m_Task.Binder);
            awaiter.OnCompleted(wraper.Run);

            ICanelTask cancelTask = awaiter as ICanelTask;
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
                StateMachineWraper<TStateMachine>.Require(ref stateMachine, m_Task, m_Task.Binder);
            awaiter.UnsafeOnCompleted(wraper.Run);

            ICanelTask cancelTask = awaiter as ICanelTask;
            if (cancelTask != null)
            {
                cancelTask.Token.AddHandler(stateMachine.MoveNext);
            }
        }

        public void SetException(Exception e)
        {
            if (e is OperationCanceledException)
            {
                Log.Debug(e.ToString());
            }
            else
            {
                XTask.ExceptionHandler.Invoke(e);
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        private void InnerCheckCancel()
        {
            if (m_Task.Binder != null)
            {
                if (m_Task.Binder.IsDisposed)
                    m_CanelTask.Token.Cancel();
            }

            m_CanelTask.Token.Invoke();
        }
    }
}
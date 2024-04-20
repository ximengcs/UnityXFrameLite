using System;
using System.Runtime.CompilerServices;
using UnityEngine;
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
            builder.m_Task.SetAction(XTaskHelper.UseAction);
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
            StateMachineWraper<TStateMachine, T> wraper =
                StateMachineWraper<TStateMachine, T>.Require(ref stateMachine, m_Task, SetResult);

            ITask task = awaiter as ITask;
            if (task == null)
            {
                Log.Warning("XFrame", "Please use new task.");
                awaiter.OnCompleted(wraper.RunNoState);
            }
            else
            {
                task.OnCompleted(wraper.Run);
                m_Task.AddChild(task);
            }
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        public void SetException(Exception e)
        {
            if (e is not OperationCanceledException)
            {
                XTask.ExceptionHandler?.Invoke(e);
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
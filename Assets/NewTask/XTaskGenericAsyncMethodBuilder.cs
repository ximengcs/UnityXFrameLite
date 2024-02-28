
using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    public struct XTaskAsyncMethodBuilder<T>
    {
        private XTask<T> m_Task;

        public XTask<T> Task => m_Task;

        public static XTaskAsyncMethodBuilder<T> Create()
        {
            XTaskAsyncMethodBuilder<T> builder = new XTaskAsyncMethodBuilder<T>();
            builder.m_Task = new XTask<T>();
            return builder;
        }

        public void SetResult(T result)
        {
            m_Task.SetResult(result);
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {

        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
        {

        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {

        }

        public void SetException(Exception e)
        {
            XTask.ExceptionHandler.Invoke(e);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }
    }
}

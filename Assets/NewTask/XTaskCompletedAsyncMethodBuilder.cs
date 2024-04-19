using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    public struct XTaskCompletedAsyncMethodBuilder
    {
        public XTaskCompleted Task => default;

        public static XTaskCompletedAsyncMethodBuilder Create()
        {
            return new XTaskCompletedAsyncMethodBuilder();
        }

        public void SetResult()
        {
            
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
            if (e is not OperationCanceledException)
            {
                XTask.ExceptionHandler?.Invoke(e);
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
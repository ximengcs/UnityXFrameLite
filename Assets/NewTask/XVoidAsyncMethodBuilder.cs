using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    public struct XVoidAsyncMethodBuilder
    {
        public XVoid Task => default;

        public static XVoidAsyncMethodBuilder Create()
        {
            return new XVoidAsyncMethodBuilder();
        }

        public void SetResult()
        {
            
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
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
    }
}
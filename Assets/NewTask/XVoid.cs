using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    [AsyncMethodBuilder(typeof(XVoidAsyncMethodBuilder))]
    public struct XVoid : ICriticalNotifyCompletion
    {
        public bool IsCompleted => true;

        public void Coroutine()
        {
            
        }
        
        public void OnCompleted(Action continuation)
        {
            
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            
        }
    }
}
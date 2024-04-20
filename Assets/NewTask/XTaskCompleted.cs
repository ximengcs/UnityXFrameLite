using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    [AsyncMethodBuilder(typeof(XTaskCompletedAsyncMethodBuilder))]
    public struct XTaskCompleted : ICriticalNotifyCompletion, ITask
    {
        public XTaskCompleted GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted => true;
        
        public float Progress => XTaskHelper.MAX_PROGRESS;
        
        public XTaskAction TaskAction => XTaskAction.CompleteWhenSubTaskFailure;

        public ITask SetAction(XTaskAction action)
        {
            return this;
        }

        public void Coroutine()
        {
        }

        public void Cancel(bool subTask)
        {
            
        }

        public ITask Bind(ITaskBinder binder)
        {
            return this;
        }

        public ITask OnCompleted(Action<XTaskState> hanlder)
        {
            if (hanlder != null)
                hanlder(XTaskState.Normal);
            
            return this;
        }

        ITask ITask.OnCompleted(Action handler)
        {
            if(handler != null)
                handler();
            return this;
        }

        public void OnCompleted(Action handler)
        {
            if(handler != null)
                handler();
        }

        public void UnsafeOnCompleted(Action handler)
        {
            if(handler != null)
                handler();
        }
    }
}
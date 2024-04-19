using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    [AsyncMethodBuilder(typeof(XVoidAsyncMethodBuilder))]
    public struct XVoid : ICriticalNotifyCompletion, ITask
    {
        public bool IsCompleted => true;
        public float Progress => XTaskHelper.MAX_PROGRESS;
        
        private XTaskAction m_TaskAction;
        public XTaskAction TaskAction => m_TaskAction;

        public ITask SetAction(XTaskAction action)
        {
            m_TaskAction = action;
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
            return this;
        }

        ITask ITask.OnCompleted(Action handler)
        {
            return this;
        }

        public void OnCompleted(Action continuation)
        {
            
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            
        }
    }
}
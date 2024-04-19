using System;

namespace XFrame.Modules.NewTasks
{
    public interface ITask
    {
        bool IsCompleted { get; }

        float Progress { get; }

        XTaskAction TaskAction { get; }

        void Coroutine();

        ITask SetAction(XTaskAction action);
        
        ITask Bind(ITaskBinder binder);
        
        void Cancel(bool subTask);

        ITask OnCompleted(Action<XTaskState> hanlder);

        ITask OnCompleted(Action handler);
    }
}
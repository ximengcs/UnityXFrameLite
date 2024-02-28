
namespace XFrame.Modules.NewTasks
{
    internal interface ICanelTask
    {
        XTaskCancelToken Token { get; }

        void Cancel();
    }
}

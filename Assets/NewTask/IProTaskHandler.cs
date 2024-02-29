namespace XFrame.Modules.NewTasks
{
    public interface IProTaskHandler
    {
        object Data { get; }

        bool IsDone { get; }

        float Pro { get; }
    }
}
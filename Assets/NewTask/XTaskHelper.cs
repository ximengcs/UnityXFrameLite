namespace XFrame.Modules.NewTasks
{
    public class XTaskHelper
    {
        public static XTaskCancelToken UseToken { get; set; }
        public static XTaskAction UseAction { get; set; }

        public const int MIN_PROGRESS = 0;
        public const int MAX_PROGRESS = 1;
    }
}
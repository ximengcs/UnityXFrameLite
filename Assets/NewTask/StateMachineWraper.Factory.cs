using System.Collections.Concurrent;

namespace XFrame.Modules.NewTasks
{
    internal partial class StateMachineWraper<T>
    {
        public const int MAX_CACHE = 1024;

        public static ConcurrentQueue<StateMachineWraper<T>>
            s_CacheQueue = new ConcurrentQueue<StateMachineWraper<T>>();

        public static StateMachineWraper<T> Require(ref T stateMachine, ICanelTask task, ITaskBinder binder)
        {
            if (!s_CacheQueue.TryDequeue(out StateMachineWraper<T> wraper))
            {
                wraper = new StateMachineWraper<T>();
            }

            wraper.m_StateMachine = stateMachine;
            wraper.m_Task = task;
            wraper.m_Binder = binder;
            return wraper;
        }

        public static void Release(StateMachineWraper<T> stateMachine)
        {
            if (stateMachine == null)
                return;
            if (s_CacheQueue.Count > MAX_CACHE)
                return;
            stateMachine.Clear();
            s_CacheQueue.Enqueue(stateMachine);
        }
    }
}
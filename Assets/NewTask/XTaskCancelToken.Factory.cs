using System.Collections.Concurrent;

namespace XFrame.Modules.NewTasks
{
    public partial class XTaskCancelToken
    {
        public const int MAX_CACHE = 1024;

        public static ConcurrentQueue<XTaskCancelToken>
            s_CacheQueue = new ConcurrentQueue<XTaskCancelToken>();

        public static XTaskCancelToken Require()
        {
            if (!s_CacheQueue.TryDequeue(out XTaskCancelToken token))
            {
                token = new XTaskCancelToken();
            }

            token.m_Canceled = false;
            return token;
        }

        public static void Release(XTaskCancelToken token)
        {
            if (token == null)
                return;
            if (s_CacheQueue.Count > MAX_CACHE)
                return;
            token.Clear();
            s_CacheQueue.Enqueue(token);
        }
    }
}
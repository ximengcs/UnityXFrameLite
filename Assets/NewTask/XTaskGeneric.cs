using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    [AsyncMethodBuilder(typeof(XTaskAsyncMethodBuilder<>))]
    public class XTask<T> : ICriticalNotifyCompletion
    {
        private Action m_OnComplete;
        private bool m_IsCompleted;
        private T m_Result;

        public bool IsCompleted => m_IsCompleted;
        public T Result => m_Result;

        public XTask()
        {

        }

        public void SetResult(T result)
        {
            m_Result = result;
            m_IsCompleted = true;
            if (m_OnComplete != null)
            {
                m_OnComplete();
                m_OnComplete = null;
            }
        }

        public XTask<T> GetAwaiter()
        {
            return this;
        }

        public void OnCompleted(Action continuation)
        {
            if (m_IsCompleted)
            {
                continuation();
            }
            else
            {
                m_OnComplete += continuation;
            }
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (m_IsCompleted)
            {
                continuation();
            }
            else
            {
                m_OnComplete += continuation;
            }
        }
    }
}

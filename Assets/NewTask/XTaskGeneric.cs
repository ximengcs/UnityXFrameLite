using System;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    [AsyncMethodBuilder(typeof(XTaskAsyncMethodBuilder<>))]
    public class XTask<T> : ICriticalNotifyCompletion, ICancelTask
    {
        private T m_Result;
        private Action m_OnComplete;
        private bool m_IsCompleted;
        private ITaskBinder m_Binder;
        private XTaskCancelToken m_CancelToken;

        XTaskCancelToken ICancelTask.Token
        {
            get
            {
                if (XTaskHelper.UseToken != null)
                    m_CancelToken = XTaskHelper.UseToken;
                else if (m_CancelToken == null)
                    m_CancelToken = XTaskCancelToken.Require();
                return m_CancelToken;
            }
        }

        ITaskBinder ICancelTask.Binder => m_Binder;

        public bool IsCompleted => m_IsCompleted;

        public XTask(XTaskCancelToken cancelToken = null)
        {
            m_CancelToken = cancelToken;
        }

        public void Coroutine()
        {
            InnerCoroutine();
        }

        private async void InnerCoroutine()
        {
            await this;
        }

        public XTask<T> Bind(ITaskBinder binder)
        {
            m_Binder = binder;
            return this;
        }

        void ICancelTask.Cancel()
        {
            m_Result = default;
            m_IsCompleted = true;
            m_OnComplete = null;
        }

        public void SetResult(T result)
        {
            if (m_CancelToken != null)
                XTaskCancelToken.Release(m_CancelToken);

            m_Result = result;
            m_IsCompleted = true;
            if (m_OnComplete != null)
            {
                m_OnComplete();
                m_OnComplete = null;
            }
        }

        public T GetResult()
        {
            return m_Result;
        }

        public XTask<T> GetAwaiter()
        {
            return this;
        }

        public void Cancel()
        {
            ICancelTask cancelTask = this;
            cancelTask.Token.Cancel();
        }

        public XTask<T> OnCancel(Action handler)
        {
            ICancelTask cancelTask = this;
            cancelTask.Token.AddHandler(handler);
            return this;
        }

        public XTask<T> OnComplete(Action handler)
        {
            if (m_IsCompleted)
            {
                handler();
            }
            else
            {
                m_OnComplete += handler;
            }

            return this;
        }

        void INotifyCompletion.OnCompleted(Action handler)
        {
            OnComplete(handler);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action handler)
        {
            OnComplete(handler);
        }
    }
}
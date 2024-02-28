using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace XFrame.Modules.NewTasks
{
    [AsyncMethodBuilder(typeof(XTaskAsyncMethodBuilder))]
    public class XTask : ICriticalNotifyCompletion, ICanelTask
    {
        public static Action<Exception> ExceptionHandler;

        private Action m_OnComplete;
        private bool m_IsCompleted;
        private ITaskBinder m_Binder;
        private XTaskCancelToken m_CancelToken;

        XTaskCancelToken ICanelTask.Token
        {
            get
            {
                if (m_CancelToken == null)
                    m_CancelToken = XTaskCancelToken.Require();
                return m_CancelToken;
            }
        }

        internal ITaskBinder Binder => m_Binder;

        public bool IsCompleted => m_IsCompleted;

        public XTask()
        {
        }

        public void Coroutine()
        {
            InnerCoroutine();
        }

        private async void InnerCoroutine()
        {
            await this;
        }

        public XTask Bind(ITaskBinder binder)
        {
            m_Binder = binder;
            return this;
        }

        void ICanelTask.Cancel()
        {
            m_IsCompleted = true;
            m_OnComplete = null;
        }

        public void SetResult()
        {
            if (m_CancelToken != null)
                XTaskCancelToken.Release(m_CancelToken);
            
            m_IsCompleted = true;
            if (m_OnComplete != null)
            {
                m_OnComplete();
                m_OnComplete = null;
            }
        }

        public void GetResult()
        {
        }

        public XTask GetAwaiter()
        {
            return this;
        }

        public void Cancel()
        {
            ICanelTask cancelTask = this;
            cancelTask.Token.Cancel();
        }
        
        public XTask OnCancel(Action handler)
        {
            ICanelTask cancelTask = this;
            cancelTask.Token.AddHandler(handler);
            return this;
        }

        public XTask OnComplete(Action handler)
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
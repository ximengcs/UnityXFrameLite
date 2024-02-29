using System;
using XFrame.Core;
using System.Runtime.CompilerServices;
using Async;

namespace XFrame.Modules.NewTasks
{
    public class XProTask : ICriticalNotifyCompletion, IUpdater
    {
        private Action m_OnComplete;
        private Action m_OnContinue;
        private Action<float> m_OnUpdate;
        private IProTaskHandler m_ProHandler;
        private XTaskCancelToken m_CancelToken;

        public bool IsCompleted => InnerCheckComplete();

        private bool InnerCheckComplete()
        {
            if (m_CancelToken.Canceled)
            {
                m_CancelToken.InvokeWithoutException();
                XTaskCancelToken.Release(m_CancelToken);
                m_CancelToken = null;
                m_OnContinue();
                m_OnComplete = null;
                m_OnContinue = null;
                m_OnUpdate = null;
                m_ProHandler = null;
                TestModule.Inst.UnRegister(this);
                return true;
            }
            else
            {
                bool done = m_ProHandler.IsDone;
                m_OnUpdate?.Invoke(Pro);
                if (done)
                {
                    m_OnContinue();
                    m_OnContinue = null;
                    if (m_OnComplete != null)
                    {
                        m_OnComplete();
                        m_OnComplete = null;
                    }

                    XTaskCancelToken.Release(m_CancelToken);
                    m_CancelToken = null;
                    m_OnUpdate = null;
                    m_ProHandler = null;
                    TestModule.Inst.UnRegister(this);
                }

                return done;
            }
        }

        public float Pro => m_ProHandler.Pro;

        public XProTask GetAwaiter()
        {
            return this;
        }

        public object GetResult()
        {
            return m_ProHandler.Data;
        }

        public XProTask(IProTaskHandler handler, XTaskCancelToken cancelToken = null)
        {
            m_ProHandler = handler;
            m_CancelToken = cancelToken;
            if (m_CancelToken == null)
                m_CancelToken = XTaskHelper.UseToken;
            if (m_CancelToken == null)
                m_CancelToken = XTaskCancelToken.Require();
            TestModule.Inst.Register(this);
        }

        public void Cancel()
        {
            m_CancelToken.Cancel();
        }

        void IUpdater.OnUpdate(float escapeTime)
        {
            InnerCheckComplete();
        }

        public void OnUpdate(Action<float> handler)
        {
            m_OnUpdate += handler;
        }

        public void OnComplete(Action handler)
        {
            if (m_ProHandler.IsDone)
                handler();
            else
                m_OnComplete += handler;
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (m_ProHandler.IsDone)
                continuation();
            else
                m_OnContinue += continuation;
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            if (m_ProHandler.IsDone)
                continuation();
            else
                m_OnContinue += continuation;
        }
    }
}
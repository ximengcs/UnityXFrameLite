using System;

namespace XFrame.Modules.NewTasks
{
    public partial class XTaskCancelToken
    {
        private bool m_Canceled;
        private Action m_Handler;
        private bool m_Disposed;

        public bool Disposed => m_Disposed;

        public bool Canceled => m_Canceled;

        private XTaskCancelToken()
        {
        }

        public void Cancel()
        {
            m_Canceled = true;
        }

        public void AddHandler(Action handler)
        {
            m_Handler += handler;
        }

        public void RemoveHandler(Action handler)
        {
            m_Handler -= handler;
        }

        public void Dispose()
        {
            m_Disposed = true;
            Clear();
        }

        public void Clear()
        {
            m_Handler = null;
        }

        public void Invoke()
        {
            if (m_Canceled)
            {
                m_Handler?.Invoke();
                InnerThrowOperationCanceledException();
            }
        }

        public void InvokeWithoutException()
        {
            if (m_Canceled)
            {
                m_Handler?.Invoke();
            }
        }

        private void InnerThrowOperationCanceledException()
        {
            throw new OperationCanceledException("OperationCanceled");
        }
    }
}
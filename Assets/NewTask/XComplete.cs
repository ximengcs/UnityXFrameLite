using System;

namespace XFrame.Modules.NewTasks
{
    public struct XComplete<T>
    {
        private bool m_Complete;
        private T m_Value;
        private Action m_Handler;
        private Action<T> m_Handler2;

        public T Value
        {
            get => m_Value;
            set => m_Value = value;
        }
        
        public bool IsComplete
        {
            get => m_Complete;
            set => m_Complete = value;
        }

        public XComplete(T defaultValue)
        {
            m_Value = defaultValue;
            m_Complete = false;
            m_Handler = null;
            m_Handler2 = null;
        }
        
        public void Invoke()
        {
            if (!m_Complete)
                return;
            if (m_Handler != null)
            {
                m_Handler.Invoke();
                m_Handler = null;
            }

            if (m_Handler2 != null)
            {
                m_Handler2.Invoke(m_Value);
                m_Handler2 = null;
            }
        }

        public void On(Action handler)
        {
            if (m_Complete)
                handler();
            else
                m_Handler += handler;
        }

        public void On(Action<T> handler)
        {
            if (m_Complete)
                handler(m_Value);
            else
                m_Handler2 += handler;
        }
    }
}
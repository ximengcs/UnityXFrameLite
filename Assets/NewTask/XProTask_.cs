using System;
using System.Runtime.CompilerServices;
using Async;
using UnityEngine;
using XFrame.Core;

namespace XFrame.Modules.NewTasks
{
    public class XProTask_ : ICriticalNotifyCompletion, IUpdater, ICancelTask, ITask
    {
        private Action<float> m_OnUpdate;
        private IProTaskHandler m_ProHandler;
        private XComplete<XTaskState> m_OnComplete;
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

        private XTaskAction m_TaskAction;
        public XTaskAction TaskAction => m_TaskAction;

        public ITask SetAction(XTaskAction action)
        {
            m_TaskAction = action;
            return this;
        }

        public object GetResult()
        {
            return m_ProHandler.Data;
        }

        public bool IsCompleted => m_OnComplete.IsComplete;

        public float Progress => m_ProHandler.Pro;

        public XProTask_(IProTaskHandler handler, XTaskCancelToken cancelToken = null)
        {
            m_ProHandler = handler;
            m_OnComplete = new XComplete<XTaskState>(XTaskState.Normal);
            m_CancelToken = cancelToken;
            TestModule.Inst.Register(this);
        }

        void IUpdater.OnUpdate(float escapeTime)
        {
            if (m_CancelToken != null)
            {
                if (!m_CancelToken.Canceled && m_OnComplete.IsComplete)
                    return;
            }
            else
            {
                if (m_OnComplete.IsComplete)
                    return;
            }

            m_OnUpdate?.Invoke(Progress);
            if (m_Binder != null && m_Binder.IsDisposed)
            {
                m_OnComplete.Value = XTaskState.BinderDispose;
                m_OnComplete.IsComplete = true;
                m_OnComplete.Invoke();
            }
            else if (m_CancelToken != null && m_CancelToken.Canceled)
            {
                m_OnComplete.Value = XTaskState.Cancel;
                m_OnComplete.IsComplete = true;
                m_OnComplete.Invoke();
            }
            else if (m_ProHandler.IsDone)
            {
                m_OnComplete.Value = XTaskState.Normal;
                m_OnComplete.IsComplete = true;
                m_OnComplete.Invoke();
            }
        }

        void ICancelTask.SetState(XTaskState state)
        {
            m_OnComplete.Value = state;
        }

        public void Coroutine()
        {
            InnerCoroutine().Coroutine();
        }

        private async XVoid InnerCoroutine()
        {
            await this;
        }

        public ITask Bind(ITaskBinder binder)
        {
            m_Binder = binder;
            return this;
        }

        public void SetResult()
        {
            Debug.LogWarning("Set Result");
            if (m_CancelToken != null && !m_CancelToken.Disposed)
                XTaskCancelToken.Release(m_CancelToken);

            m_OnUpdate = null;
            TestModule.Inst.UnRegister(this);
        }

        public XProTask_ GetAwaiter()
        {
            return this;
        }

        public void Cancel(bool subTask)
        {
            InnerCancel();
        }

        private void InnerCancel()
        {
            if (m_OnComplete.IsComplete)
                return;
            m_OnComplete.IsComplete = true;

            ICancelTask cancelTask = this;
            cancelTask.Token.Cancel();
        }


        public ITask OnUpdate(Action<float> handler)
        {
            m_OnUpdate += handler;
            return this;
        }


        public ITask OnCompleted(Action<XTaskState> handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

        public ITask OnCompleted(Action handler)
        {
            m_OnComplete.On(handler);
            return this;
        }

        void INotifyCompletion.OnCompleted(Action handler)
        {
            m_OnComplete.On(handler);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action handler)
        {
            m_OnComplete.On(handler);
        }
    }
}
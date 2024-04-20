using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XFrame.Modules.NewTasks
{
    [AsyncMethodBuilder(typeof(XTaskAsyncMethodBuilder))]
    public class XTask : ICriticalNotifyCompletion, ICancelTask, ITask
    {
        public static Action<Exception> ExceptionHandler;

        private XComplete<XTaskState> m_OnComplete;
        private ITaskBinder m_Binder;
        private XTaskCancelToken m_CancelToken;
        private List<ITask> m_Children;

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
        
        public bool IsCompleted => m_OnComplete.IsComplete;

        public float Progress => m_OnComplete.IsComplete ? XTaskHelper.MAX_PROGRESS : XTaskHelper.MIN_PROGRESS;

        public XTask(XTaskCancelToken cancelToken = null)
        {
            m_OnComplete = new XComplete<XTaskState>(XTaskState.Normal);
            m_CancelToken = cancelToken;
            m_Children = new List<ITask>();
        }

        internal void AddChild(ITask task)
        {
            m_Children.Add(task);
        }

        void ICancelTask.SetState(XTaskState state)
        {
            m_OnComplete.Value = state;
        }

        public void Coroutine() 
        {
            InnerCoroutine();
        }

        private async void InnerCoroutine()
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
            if (m_CancelToken != null && !m_CancelToken.Disposed)
                XTaskCancelToken.Release(m_CancelToken);

            m_OnComplete.IsComplete = true;
            m_OnComplete.Invoke();
        }

        public void GetResult()
        {
        }

        public XTask GetAwaiter()
        {
            return this;
        }

        public void Cancel(bool subTask)
        {
            InnerCancel(subTask);
        }

        private void InnerCancel(bool subTask)
        {
            if (m_OnComplete.IsComplete)
                return;
            m_OnComplete.IsComplete = true;

            if (subTask)
            {
                foreach (ITask task in m_Children)
                {
                    task.Cancel(subTask);
                }
            }

            ICancelTask cancelTask = this;
            cancelTask.Token.Cancel();
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
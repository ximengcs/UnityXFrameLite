using XFrame.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.HotUpdate
{
    public class HotUpdateCheckTask : XProTask
    {
        private Handler m_Handler;

        public bool Success { get; private set; }
        public List<string> ResList { get; private set; }

        public HotUpdateCheckTask() : base(null)
        {
            m_Handler = new Handler(this);
            m_ProHandler = m_Handler;
        }

        protected override void InnerStart()
        {
            base.InnerStart();
            m_Handler.Start();
        }

        private class Handler : IProTaskHandler
        {
            private double m_Pro;
            private HotUpdateCheckTask m_Task;
            private AsyncOperationHandle<List<string>> m_Op;

            public object Data => throw new System.NotImplementedException();

            public bool IsDone
            {
                get
                {
                    if (!m_Op.IsDone)
                    {
                        m_Pro = m_Op.PercentComplete;
                    }
                    else
                    {
                        if (m_Op.IsValid() && m_Op.Status == AsyncOperationStatus.Succeeded)
                        {
                            m_Task.Success = true;
                            m_Task.ResList = m_Op.Result;
                        }
                        else
                        {
                            m_Task.Success = false;
                        }

                        m_Pro = XTaskHelper.MAX_PROGRESS;
                    }

                    return m_Pro >= XTaskHelper.MAX_PROGRESS;
                }
            }

            public double Pro => m_Pro;

            public Handler(HotUpdateCheckTask task)
            {
                m_Task = task;
            }

            public void Start()
            {
                m_Op = Addressables.CheckForCatalogUpdates(false);
            }

            public void Dispose()
            {
                Addressables.Release(m_Op);
            }

            public void OnCancel()
            {
                Dispose();
            }
        }
    }
}

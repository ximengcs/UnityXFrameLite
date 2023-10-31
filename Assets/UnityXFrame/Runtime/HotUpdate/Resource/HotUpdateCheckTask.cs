using XFrame.Modules.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.HotUpdate
{
    public class HotUpdateCheckTask : TaskBase
    {
        public bool Success { get; private set; }
        public List<string> ResList { get; private set; }

        protected override void OnCreateFromPool()
        {
            base.OnCreateFromPool();
            AddStrategy(new Strategy());
            Add(new Handler());
        }

        private class Strategy : ITaskStrategy<Handler>
        {
            private Handler m_Handler;

            public void OnUse(Handler handler)
            {
                m_Handler = handler;
                m_Handler.Start();
            }

            public float OnHandle(ITask from)
            {
                if (!m_Handler.Op.IsDone)
                {
                    return m_Handler.Op.PercentComplete;
                }
                else
                {
                    HotUpdateCheckTask task = (HotUpdateCheckTask)from;
                    if (m_Handler.Op.IsValid() && m_Handler.Op.Status == AsyncOperationStatus.Succeeded)
                    {
                        task.Success = true;
                        task.ResList = m_Handler.Op.Result;
                    }
                    else
                    {
                        task.Success = false;
                    }

                    return MAX_PRO;
                }
            }

            public void OnFinish()
            {
                m_Handler.Dispose();
                m_Handler = null;
            }
        }

        private class Handler : ITaskHandler
        {
            public AsyncOperationHandle<List<string>> Op { get; private set; }

            public void Start()
            {
                Op = Addressables.CheckForCatalogUpdates(false);
            }

            public void Dispose()
            {
                Addressables.Release(Op);
            }
        }
    }
}

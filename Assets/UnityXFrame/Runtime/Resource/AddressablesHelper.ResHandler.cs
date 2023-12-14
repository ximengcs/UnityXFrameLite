using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper
    {
        private class ResHandler : IAddresableResHandler
        {
            private AsyncOperationHandle m_Handle;
            private object m_Data;

            public object Data
            {
                get
                {
                    if (m_Data == null)
                        m_Data = m_Handle.Result;
                    return m_Data;
                }
            }

            public bool IsDone => m_Handle.IsDone;

            public float Pro => m_Handle.PercentComplete;

            public ResHandler(AsyncOperationHandle handle)
            {
                m_Handle = handle;
            }

            public void Start()
            {
                m_Data = m_Handle.WaitForCompletion();
            }

            public void Dispose()
            {

            }

            public void Release()
            {
                Addressables.Release(m_Handle);
                m_Data = null;
            }
        }
    }
}

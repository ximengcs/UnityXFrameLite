using System;
using XFrame.Modules.Resource;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper
    {
        private interface IAddresableResHandler : IResHandler
        {
            void Release();
        }

        private class AsyncResHandler : IAddresableResHandler
        {
            private AsyncOperationHandle m_Handle;
            private object m_Data;
            private string m_Path;
            private Type m_Type;

            public object Data
            {
                get
                {
                    if (m_Data == null)
                        m_Data = m_Handle.Result;
                    return m_Data;
                }
            }

            public string AssetPath => m_Path;

            public Type AssetType => m_Type;

            public bool IsDone => m_Handle.IsDone;

            public double Pro => m_Handle.PercentComplete;

            public AsyncResHandler(AsyncOperationHandle handle, string path, Type type)
            {
                m_Handle = handle;
                m_Path = path;
                m_Type = type;
            }

            public void Start()
            {
                InnerStart();
            }

            public void OnCancel()
            {
                Release();
            }

            public void Dispose()
            {
                Release();
            }

            public void Release()
            {
                Addressables.Release(m_Handle);
                m_Data = null;
            }

            private async void InnerStart()
            {
                await m_Handle.Task;
            }
        }
    }
}

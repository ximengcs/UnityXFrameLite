using System;
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
            private string m_AssetPath;
            private Type m_Type;

            public string AssetPath => m_AssetPath;

            public Type AssetType => m_Type;

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

            public ResHandler(AsyncOperationHandle handle, string assetPath, Type type)
            {
                m_Handle = handle;
                m_AssetPath = assetPath;
                m_Type = type;
            }

            public void Start()
            {
                m_Data = m_Handle.WaitForCompletion();
            }

            public void OnCancel()
            {
                Release();
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

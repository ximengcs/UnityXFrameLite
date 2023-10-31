using System;
using XFrame.Modules.Diagnotics;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace UnityXFrame.Core.HotUpdate
{
    public partial class HotUpdateDownTask
    {
        private class DownLoadInfo
        {
            private string m_Key;
            private float m_Size;
            private AsyncOperationHandle m_OpHandle;
            private Action<string> m_OnComplete;

            public float Pro => m_OpHandle.PercentComplete;

            public float Size => m_Size;

            public DownLoadInfo(string key, AsyncOperationHandle opHandle)
            {
                m_Key = key;
                m_OpHandle = opHandle;
                m_OpHandle.Completed += InnerCompleteHandler;
            }

            public DownLoadInfo(string key)
            {
                m_Key = key;
            }

            internal void SetSize(float size)
            {
                m_Size = size;
            }

            internal void SetHandle(AsyncOperationHandle opHandle)
            {
                m_OpHandle = opHandle;
                m_OpHandle.Completed += InnerCompleteHandler;
            }

            public void OnComplete(Action<string> callback)
            {
                if (m_OpHandle.IsDone && m_OpHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(m_Key);
                }
                else
                {
                    m_OnComplete += callback;
                }
            }

            private void InnerCompleteHandler(AsyncOperationHandle op)
            {
                m_OnComplete?.Invoke(m_Key);
                m_OnComplete = null;
            }
        }
    }
}

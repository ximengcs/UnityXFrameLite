using System;
using XFrame.Modules.Diagnotics;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.HotUpdate
{
    public partial class HotUpdateDownTask
    {
        private class DownLoadInfo
        {
            private string m_Key;
            private AsyncOperationHandle m_OpHandle;
            private Action m_OnComplete;

            public float Pro => m_OpHandle.PercentComplete;

            public DownLoadInfo(string key, AsyncOperationHandle opHandle)
            {
                m_Key = key;
                m_OpHandle = opHandle;
                m_OpHandle.Completed += InnerCompleteHandler;
                Log.Debug("XFrame", $"DownloadInfo {m_Key} Start");
            }

            public void OnComplete(Action callback)
            {
                m_OnComplete += callback;
            }

            private void InnerCompleteHandler(AsyncOperationHandle op)
            {
                Log.Debug("XFrame", $"DownloadInfo {m_Key} Complete");
                m_OnComplete?.Invoke();
                m_OnComplete = null;
            }
        }
    }
}

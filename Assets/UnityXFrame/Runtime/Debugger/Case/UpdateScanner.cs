using UnityEngine;
using XFrame.Modules.Tasks;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using UnityXFrame.Core.HotUpdate;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using XFrame.Core;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugHelp("view resource update progress")]
    [DebugWindow(-999, "Resource")]
    public class UpdateScanner : IDebugWindow
    {
        private HotUpdateCheckTask m_CheckTask;
        private HotUpdateDownTask m_DownTask;
        private List<string> m_CachePath;
        private string m_CheckKey;
        private Vector2 m_Pos;

        public void OnAwake()
        {
            m_CachePath = new List<string>();
            Caching.GetAllCachePaths(m_CachePath);
            m_DownTask = XModule.Task.Get<HotUpdateDownTask>(Constant.UPDATE_RES_TASK);
            m_CheckTask = XModule.Task.Get<HotUpdateCheckTask>(Constant.UPDATE_CHECK_TASK);
        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Clear Cache"))
                Caching.ClearCache();
            if (DebugGUI.Button("Update Resource"))
                InnerUpdateRes();

            DebugGUI.Label("Check Progress");
            GUILayout.BeginHorizontal();
            if (m_CheckTask != null)
                DebugGUI.Progress(m_CheckTask.Pro);
            else
                DebugGUI.Progress(1);
            GUILayout.EndHorizontal();

            DebugGUI.Label("Down Progress");
            GUILayout.BeginHorizontal();
            if (m_DownTask != null)
                DebugGUI.Progress(m_DownTask.Pro);
            else
                DebugGUI.Progress(1);
            GUILayout.EndHorizontal();

            DebugGUI.Label("Cache Path");
            m_Pos = DebugGUI.BeginScrollView(m_Pos, DebugGUI.Height(200));
            foreach (string path in m_CachePath)
            {
                GUILayout.Box(new GUIContent(path));
                if (DebugGUI.Button("Copy"))
                    GUIUtility.systemCopyBuffer = path;
            }
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            m_CheckKey = DebugGUI.TextField(m_CheckKey);
            if (DebugGUI.Button("CheckUpdate"))
                InnerCheck();
            GUILayout.EndHorizontal();
        }

        public void Dispose()
        {

        }

        private void InnerUpdateRes()
        {
            Log.Debug("Start hot update check task.");
            m_CheckTask = XModule.Task.GetOrNew<HotUpdateCheckTask>(Constant.UPDATE_CHECK_TASK);
            m_CheckTask.OnComplete(() =>
            {
                if (m_CheckTask.Success)
                    Log.Debug($"Hot update check task has success.");
                else
                    Log.Debug("Hot update check task has failure.");
                Log.Debug("Start hot update download task.");
                m_DownTask = XModule.Task.GetOrNew<HotUpdateDownTask>(Constant.UPDATE_RES_TASK);
                m_DownTask.AddList(m_CheckTask.ResList).OnComplete(() =>
                {
                    if (m_DownTask.Success)
                        Log.Debug("Hot update download task has success.");
                    else
                        Log.Debug("Hot update download task has failure.");
                }).Start();
            }).Start();
        }

        private void InnerCheck()
        {
            if (!string.IsNullOrEmpty(m_CheckKey))
            {
                AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(m_CheckKey);
                sizeHandle.Completed += (handle) =>
                {
                    if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Log.Debug($"size is {sizeHandle.Result}");
                    }
                    else
                    {
                        Log.Debug("size check failure");
                    }
                };

            }
        }
    }
}

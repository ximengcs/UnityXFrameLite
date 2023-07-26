using UnityEngine;
using XFrame.Modules.Tasks;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using UnityXFrame.Core.HotUpdate;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugHelp("view resource update progress")]
    [DebugWindow(-999, "ResUpdate")]
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
            m_DownTask = TaskModule.Inst.Get<HotUpdateDownTask>(Constant.UPDATE_RES_TASK);
            m_CheckTask = TaskModule.Inst.Get<HotUpdateCheckTask>(Constant.UPDATE_CHECK_TASK);
        }

        public void OnDraw()
        {
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
            m_Pos = DebugGUI.BeginScrollView(m_Pos, GUILayout.Height(200));
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

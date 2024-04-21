using System;
using XFrame.Tasks;
using System.Collections;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace UnityXFrame.Core.HotUpdate
{
    public partial class HotUpdateDownTask : XProTask
    {
        private Handler m_Handler;
        private Action m_OnStart;
        private bool m_Staring;

        public bool Success { get; private set; }

        public bool Starting
        {
            get => m_Staring;
            internal set
            {
                if (m_Staring)
                    return;

                m_Staring = true;
                m_OnStart?.Invoke();
                m_OnStart = null;
            }
        }

        public HotUpdateDownTask() : base (null)
        {

        }

        public void OnStart(Action callback)
        {
            if (m_Staring)
            {
                callback?.Invoke();
            }
            else
            {
                m_OnStart += callback;
            }
        }

        public bool CheckExist(string key)
        {
            return m_Handler.Containes(key);
        }

        public void OnComplete(string key, Action<string> callback)
        {
            if (m_Handler.Containes(key))
            {
                m_Handler.OnComplete(key, callback);
                return;
            }
        }

        public HotUpdateDownTask AddList(List<string> downList, HashSet<string> perchs = null)
        {
            m_Handler = new Handler(this, downList, perchs);
            Starting = true;
            return this;
        }

        private enum State
        {
            Downloading,
            DownloadFailure,
            DownloadSuccess
        }

        private class Handler : IProTaskHandler
        {
            private HotUpdateDownTask m_ParentTask;
            private List<string> m_CheckList;
            private HashSet<string> m_Perchs;
            private float m_Pro;

            private Dictionary<string, DownLoadInfo> m_DownloadingDependency;
            private HashSet<AsyncOperationHandle> m_Handles;

            public State State { get; private set; }
            public float Pro => m_Pro;
            public object Data => null;
            public bool IsDone
            {
                get
                {
                    switch (State)
                    {
                        case State.Downloading:
                            if (m_Pro >= XTaskHelper.MAX_PROGRESS)
                                m_Pro = 0.988888f;
                            break;

                        case State.DownloadSuccess:
                            m_ParentTask.Success = true;
                            m_Pro = XTaskHelper.MAX_PROGRESS;
                            break;

                        case State.DownloadFailure:
                            m_ParentTask.Success = false;
                            m_Pro = XTaskHelper.MAX_PROGRESS;
                            break;

                        default:
                            m_Pro = XTaskHelper.MAX_PROGRESS;
                            break;
                    }

                    return m_Pro >= XTaskHelper.MAX_PROGRESS;
                }
            }

            public Handler(HotUpdateDownTask task, List<string> downList, HashSet<string> perchs)
            {
                m_ParentTask = task;
                m_CheckList = downList;
                m_Perchs = perchs;
                m_Handles = new HashSet<AsyncOperationHandle>();
                m_DownloadingDependency = new Dictionary<string, DownLoadInfo>();
                Download();
            }

            public void OnCancel()
            {

            }

            public void Download()
            {
                State = State.Downloading;
                if (m_CheckList != null && m_CheckList.Count > 0)
                {
                    AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(m_CheckList, true);
                    updateHandle.Completed += (handle) =>
                    {
                        if (handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            Log.Debug("XFrame", "UpdateCatalogs success.");
                        }
                        else
                        {
                            Log.Debug("XFrame", "UpdateCatalogs failure, cant download res.");
                            State = State.DownloadFailure;
                        }

                        InnerInit();
                    };
                }
                else
                {
                    State = State.DownloadSuccess;
                }
            }

            private void InnerInit()
            {
                AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
                initHandle.Completed += (handle) =>
                {
                    Log.Debug("XFrame", $"Initialize success");
                    IResourceLocator locator = handle.Result;
                    InnerCheckSize(locator.Keys);
                };
            }

            private void InnerCheckSize(IEnumerable<object> keys)
            {
                List<object> list = new List<object>(keys);
                int count = list.Count;
                list.Clear();
                foreach (object key in keys)
                {
                    if (m_Perchs != null)
                    {
                        if (!m_Perchs.Contains((string)key))
                        {
                            count--;
                            continue;
                        }
                    }

                    AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(key);
                    sizeHandle.Completed += (handle) =>
                    {
                        if (handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            long downSize = handle.Result;
                            if (downSize > 0)
                            {
                                string keyStr = (string)key;
                                if (!m_DownloadingDependency.TryGetValue(keyStr, out DownLoadInfo info))
                                {
                                    info = new DownLoadInfo(keyStr);
                                    m_DownloadingDependency.Add(keyStr, info);
                                }
                                else
                                {
                                    info.SetSize(downSize);
                                }
                                list.Add(key);
                            }

                            count--;
                            if (count == 0)
                            {
                                if (State != State.DownloadFailure)
                                {
                                    if (list.Count > 0)
                                        InnerDownloadWithEnum(list);
                                    else
                                        State = State.DownloadSuccess;
                                }
                            }
                        }
                        else
                        {
                            Log.Debug("XFrame", $"Get size failure, cant download res.");
                            State = State.DownloadFailure;
                        }
                        Addressables.Release(handle);
                    };
                }

            }

            static List<IResourceLocation> GatherDependenciesFromLocations(IList<IResourceLocation> locations)
            {
                var locHash = new HashSet<IResourceLocation>();
                foreach (var loc in locations)
                {
                    if (loc.ResourceType == typeof(IAssetBundleResource))
                    {
                        locHash.Add(loc);
                    }
                    if (loc.HasDependencies)
                    {
                        foreach (var dep in loc.Dependencies)
                            if (dep.ResourceType == typeof(IAssetBundleResource))
                                locHash.Add(dep);
                    }
                }
                return new List<IResourceLocation>(locHash);
            }

            private void InnerDownload(List<object> keys)
            {
                if (m_Perchs != null)
                {
                    AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync((IEnumerable)keys, Addressables.MergeMode.Union);
                    locationHandle.Completed += (handle) =>
                    {
                        List<IResourceLocation> filterList = new List<IResourceLocation>();
                        List<IResourceLocation> list = GatherDependenciesFromLocations(handle.Result);
                        foreach (IResourceLocation location in list)
                        {
                            if (m_Perchs.Contains(location.InternalId))
                            {
                                filterList.Add(location);
                            }
                        }

                        InnerDownloadWithEnum(filterList);
                        Addressables.Release(handle);
                    };
                }
                else
                {
                    InnerDownloadWithEnum(keys);
                }
            }

            private void InnerDownloadWithEnum(IList<IResourceLocation> locations)
            {
                AsyncOperationHandle<IList<IAssetBundleResource>> downHandle = Addressables.LoadAssetsAsync<IAssetBundleResource>(locations, null, true);
                XTask.Condition(() =>
                {
                    bool isDone = downHandle.IsDone;
                    if (isDone)
                    {
                        if (downHandle.IsValid() && downHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            State = State.DownloadSuccess;
                            Log.Debug("XFrame", $"Download success.");
                        }
                        else
                        {
                            State = State.DownloadFailure;
                            Log.Debug("XFrame", $"Download failure, can't download res. {downHandle.OperationException}");
                        }
                        m_Pro = 1;
                        Addressables.Release(downHandle);
                    }
                    else
                    {
                        m_Pro = downHandle.PercentComplete;
                    }
                    return isDone;
                }).Coroutine();
            }

            public bool Containes(string key)
            {
                return m_DownloadingDependency.ContainsKey(key);
            }

            public void OnComplete(string key, Action<string> callback)
            {
                if (m_DownloadingDependency.TryGetValue(key, out DownLoadInfo dependency))
                {
                    dependency.OnComplete(callback);
                }
            }

            private void InnerDownloadWithEnum(List<object> keys)
            {
                List<AsyncOperationHandle> waitHandles = new List<AsyncOperationHandle>();
                foreach (object key in keys)
                {
                    AsyncOperationHandle downHandle = Addressables.DownloadDependenciesAsync(key);
                    waitHandles.Add(downHandle);
                    if (!m_Handles.Contains(downHandle))
                    {
                        m_Handles.Add(downHandle);
                    }
                }

                for (int i = 0; i < keys.Count; i++)
                {
                    AsyncOperationHandle waitHandle = waitHandles[i];
                    string key = (string)keys[i];
                    if (!m_DownloadingDependency.TryGetValue(key, out DownLoadInfo info))
                    {
                        info = new DownLoadInfo(key);
                        m_DownloadingDependency.Add(key, info);
                    }
                    else
                    {
                        info.SetHandle(waitHandle);
                    }
                }

                XTask.Condition(() =>
                {
                    bool isDone = true;
                    foreach (AsyncOperationHandle handle in m_Handles)
                    {
                        if (!handle.IsDone)
                        {
                            isDone = false;
                            break;
                        }
                    }
                    if (isDone)
                    {
                        bool valid = true;
                        bool success = true;

                        foreach (AsyncOperationHandle handle in m_Handles)
                        {
                            if (!handle.IsValid())
                            {
                                valid = false;
                                Log.Debug("XFrame", $"Download failure, can't download res. {handle.OperationException}");
                                break;
                            }
                            if (handle.Status != AsyncOperationStatus.Succeeded)
                            {
                                success = false;
                                Log.Debug("XFrame", $"Download failure, can't download res. {handle.OperationException}");
                                break;
                            }
                        }

                        if (valid && success)
                        {
                            State = State.DownloadSuccess;
                            Log.Debug("XFrame", $"Download success.");
                        }
                        else
                        {
                            State = State.DownloadFailure;
                            Log.Debug("XFrame", $"Download failure, can't download res.");
                        }
                        m_Pro = 1;
                        foreach (AsyncOperationHandle handle in m_Handles)
                            Addressables.Release(handle);
                    }
                    else
                    {
                        float pro = 0;
                        float rate = 1f / m_Handles.Count;
                        foreach (AsyncOperationHandle handle in m_Handles)
                            pro += handle.PercentComplete * rate;
                        m_Pro = pro;
                    }
                    return isDone;
                }).Coroutine();

                m_ParentTask.Starting = true;
            }
        }
    }
}

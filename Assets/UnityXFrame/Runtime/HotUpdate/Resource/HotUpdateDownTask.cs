﻿using System.Collections;
using XFrame.Modules.Tasks;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.HotUpdate
{
    public class HotUpdateDownTask : TaskBase
    {
        public bool Success { get; private set; }

        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public HotUpdateDownTask AddList(List<string> downList)
        {
            Add(new Handler(downList));
            return this;
        }

        private class Strategy : ITaskStrategy<Handler>
        {
            private Handler m_Handler;

            public void OnUse(Handler handler)
            {
                m_Handler = handler;
                m_Handler.Download();
            }

            public float OnHandle(ITask from)
            {
                switch (m_Handler.State)
                {
                    case State.Downloading:
                        float pro = m_Handler.Pro;
                        if (pro >= MAX_PRO)
                            pro = 0.988888f;
                        return pro;

                    case State.DownloadSuccess:
                        HotUpdateDownTask task = (HotUpdateDownTask)from;
                        task.Success = true;
                        return MAX_PRO;

                    case State.DownloadFailure:
                        task = (HotUpdateDownTask)from;
                        task.Success = false;
                        return MAX_PRO;

                    default: return MAX_PRO;
                }
            }

            public void OnFinish()
            {
                m_Handler = null;
            }
        }

        private enum State
        {
            Downloading,
            DownloadFailure,
            DownloadSuccess
        }

        private class Handler : ITaskHandler
        {
            private List<string> m_CheckList;

            public State State { get; private set; }
            public float Pro { get; private set; }

            public Handler(List<string> downList)
            {
                m_CheckList = downList;
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
                    InnerInit();
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
                    AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(key);
                    sizeHandle.Completed += (handle) =>
                    {
                        if (handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            long downSize = handle.Result;
                            if (downSize > 0)
                            {
                                Log.Debug("XFrame", $"Require {key} download size: {downSize}");
                                list.Add(key);
                            }

                            count--;
                            if (count == 0)
                            {
                                if (State != State.DownloadFailure)
                                {
                                    if (list.Count > 0)
                                        InnerDownload(list);
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

            private void InnerDownload(IEnumerable keys)
            {
                AsyncOperationHandle downHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
                BolActionTask task = TaskModule.Inst.GetOrNew<BolActionTask>();
                task.Add(() =>
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
                        Pro = 1;
                        Addressables.Release(downHandle);
                    }
                    else
                    {
                        Pro = downHandle.PercentComplete;
                    }
                    return isDone;
                }).Start();
            }
        }
    }
}

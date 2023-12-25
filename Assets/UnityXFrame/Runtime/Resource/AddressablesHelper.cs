using System;
using XFrame.Modules.Resource;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper : IResourceHelper
    {
        private IResRedirectHelper m_DirectHelper;
        private Dictionary<int, IAddresableResHandler> m_LoadMap;

        void IResourceHelper.OnInit(string rootPath)
        {
            Ext.OnInit();
            m_LoadMap = new Dictionary<int, IAddresableResHandler>();
        }

        public void SetResDirectHelper(IResRedirectHelper helper)
        {
            if (m_DirectHelper == null)
            {
                m_DirectHelper = helper;
                m_DirectHelper.OnInit(null);
            }
            else
            {
                m_DirectHelper.SetResDirectHelper(helper);
            }
        }

        public object Load(string resPath, Type type)
        {
            if (m_DirectHelper != null)
            {
                if (m_DirectHelper.Redirect(resPath, type, out string newResPath))
                    return m_DirectHelper.Load(resPath, type);
                else
                    resPath = newResPath;
            }

            object handle = Ext.LoadAssetAsync(resPath, type);
            ReflectResHandler handler = new ReflectResHandler(handle, type);
            handler.Start();
            object asset = handler.Data;
            if (asset != null)
            {
                int code = asset.GetHashCode();
                if (!m_LoadMap.ContainsKey(code))
                    m_LoadMap.Add(code, handler);
            }
            
            return asset;
        }

        public T Load<T>(string resPath)
        {
            if (m_DirectHelper != null)
            {
                if (m_DirectHelper.Redirect(resPath, typeof(T), out string newResPath))
                    return m_DirectHelper.Load<T>(resPath);
                else
                    resPath = newResPath;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(resPath);
            ResHandler handler = new ResHandler(handle);
            handler.Start();
            T asset = (T)handler.Data;
            if (asset != null)
            {
                int code = asset.GetHashCode();
                if (!m_LoadMap.ContainsKey(code))
                    m_LoadMap.Add(code, handler);
            }

            return asset;
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            if (m_DirectHelper != null)
            {
                if (m_DirectHelper.Redirect(resPath, type, out string newResPath))
                    return m_DirectHelper.LoadAsync(resPath, type);
                else
                    resPath = newResPath;
            }

            ResLoadTask loadTask = Global.Task.GetOrNew<ResLoadTask>();
            object handle = Ext.LoadAssetAsync(resPath, type);
            ReflectAsyncResHandler handler = new ReflectAsyncResHandler(handle, type);
            loadTask.OnComplete((asset) =>
            {
                if (asset != null)
                {
                    int code = asset.GetHashCode();
                    if (!m_LoadMap.ContainsKey(code))
                        m_LoadMap.Add(code, handler);
                }
            });
            loadTask.Add(handler);
            return loadTask;
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            if (m_DirectHelper != null)
            {
                if (m_DirectHelper.Redirect(resPath, typeof(T), out string newResPath))
                    return m_DirectHelper.LoadAsync<T>(resPath);
                else
                    resPath = newResPath;
            }

            ResLoadTask<T> loadTask = Global.Task.GetOrNew<ResLoadTask<T>>();
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(resPath);
            AsyncResHandler handler = new AsyncResHandler(handle);
            loadTask.OnComplete((asset) =>
            {
                if (asset != null)
                {
                    int code = asset.GetHashCode();
                    if (!m_LoadMap.ContainsKey(code))
                        m_LoadMap.Add(code, handler);
                }
            });
            loadTask.Add(handler);
            return loadTask;
        }

        public void Unload(object target)
        {
            int code = target.GetHashCode();
            if (m_LoadMap.TryGetValue(code, out IAddresableResHandler handler))
            {
                handler.Release();
                m_LoadMap.Remove(code);
            }
        }

        public void UnloadAll()
        {
            foreach (AsyncResHandler handler in m_LoadMap.Values)
                handler.Release();
            m_LoadMap.Clear();
        }

        public List<object> DumpAll()
        {
            List<object> list = new List<object>(m_LoadMap.Count);
            foreach (var entry in m_LoadMap)
            {
                list.Add(entry.Value.Data);
            }
            return list;
        }
    }
}

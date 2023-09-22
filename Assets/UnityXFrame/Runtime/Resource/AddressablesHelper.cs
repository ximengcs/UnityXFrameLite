using System;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using XFrame.Core;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper : IResourceHelper
    {
        private Dictionary<int, IAddresableResHandler> m_LoadMap;

        void IResourceHelper.OnInit(string rootPath)
        {
            Ext.OnInit();
            m_LoadMap = new Dictionary<int, IAddresableResHandler>();
        }

        public object Load(string resPath, Type type)
        {
            return default;
        }

        public T Load<T>(string resPath)
        {
            return default;
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            ResLoadTask loadTask = Module.Task.GetOrNew<ResLoadTask>();
            object handle = Ext.LoadAssetAsync(resPath, type);
            ReflectResHandler handler = new ReflectResHandler(handle, type);
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
            ResLoadTask<T> loadTask = Module.Task.GetOrNew<ResLoadTask<T>>();
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(resPath);
            ResHandler handler = new ResHandler(handle);
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
            foreach (ResHandler handler in m_LoadMap.Values)
                handler.Release();
            m_LoadMap.Clear();
        }
    }
}

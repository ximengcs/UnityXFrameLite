using System;
using UnityEditor;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;
using XFrame.Core;
using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class EditorAssetsHelper : IResourceHelper
    {
        private FileNode<object> m_ResCache;

        void IResourceHelper.OnInit(string rootPath)
        {
            m_ResCache = new FileNode<object>(string.Empty);
        }

        public T Load<T>(string resPath)
        {
            if (m_ResCache.TryGetFile(resPath, out object res))
            {
                return (T)res;
            }
            else
            {
                Type type = typeof(T);
                T resInst = (T)(object)AssetDatabase.LoadAssetAtPath(resPath, type);
                m_ResCache.Add(resPath, resInst);
                return resInst;
            }
        }

        public object Load(string resPath, Type type)
        {
            if (m_ResCache.TryGetFile(resPath, out object res))
            {
                return res;
            }
            else
            {
                object resInst = AssetDatabase.LoadAssetAtPath(resPath, type);
                m_ResCache.Add(resPath, resInst);
                return resInst;
            }
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            ResLoadTask<T> loadTask = Global.Task.GetOrNew<ResLoadTask<T>>();
            if (m_ResCache.TryGetFile(resPath, out object res))
            {
                loadTask.Add(new ResHandler(res));
                loadTask.Start();
            }
            else
            {
                Type type = typeof(T);
                T resInst = (T)(object)AssetDatabase.LoadAssetAtPath(resPath, type);
                m_ResCache.Add(resPath, resInst);
                loadTask.Add(new ResHandler(resInst));
                loadTask.Start();
            }

            return loadTask;
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            ResLoadTask loadTask = Global.Task.GetOrNew<ResLoadTask>();
            if (m_ResCache.TryGetFile(resPath, out object res))
            {
                loadTask.Add(new ResHandler(res));
                loadTask.Start();
            }
            else
            {
                object resInst = AssetDatabase.LoadAssetAtPath(resPath, type);
                m_ResCache.Add(resPath, resInst);
                loadTask.Add(new ResHandler(resInst));
                loadTask.Start();
            }

            return loadTask;
        }

        public void Unload(object package)
        {
            m_ResCache.Remove((string)package);
        }

        public void UnloadAll()
        {
            m_ResCache.Clear();
        }
    }
}
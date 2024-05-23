using System;
using UnityEditor;
using XFrame.Modules.Resource;
using System.Collections.Generic;

namespace UnityXFrame.Editor
{
    public partial class EditorAssetsHelper : IResourceHelper
    {
        private FileNode<object> m_ResCache;

        void IResourceHelper.OnInit(string rootPath)
        {
            m_ResCache = new FileNode<object>(string.Empty);
        }

        public void SetResDirectHelper(IResRedirectHelper helper)
        {

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
            ResLoadTask<T> loadTask;
            if (m_ResCache.TryGetFile(resPath, out object res))
            {
                loadTask = new ResLoadTask<T>(new ResHandler(res, resPath, typeof(T)));
            }
            else
            {
                Type type = typeof(T);
                T resInst = (T)(object)AssetDatabase.LoadAssetAtPath(resPath, type);
                m_ResCache.Add(resPath, resInst);
                loadTask = new ResLoadTask<T>(new ResHandler(resInst, resPath, typeof(T)));
            }

            return loadTask;
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            ResLoadTask loadTask;
            if (m_ResCache.TryGetFile(resPath, out object res))
            {
                loadTask = new ResLoadTask(new ResHandler(res, resPath, type));
            }
            else
            {
                object resInst = AssetDatabase.LoadAssetAtPath(resPath, type);
                m_ResCache.Add(resPath, resInst);
                loadTask = new ResLoadTask(new ResHandler(resInst, resPath, type));
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

        public List<object> DumpAll()
        {
            throw new NotImplementedException();
        }
    }
}
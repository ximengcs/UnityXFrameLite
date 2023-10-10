using System;
using System.IO;
using UnityEngine;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;
using XFrame.Core;

namespace UnityXFrame.Core.Resource
{
    public partial class ResourcesHelper : IResourceHelper
    {
        private string m_AssetPath;

        void IResourceHelper.OnInit(string rootPath)
        {
            m_AssetPath = "Assets/Resources/";
        }

        public object Load(string resPath, Type type)
        {
            resPath = InnerCheckName(resPath);
            return Resources.Load(resPath, type);
        }

        public T Load<T>(string resPath)
        {
            resPath = InnerCheckName(resPath);
            return (T)Load(resPath, typeof(T));
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            resPath = InnerCheckName(resPath);
            ResLoadTask task = Global.Task.GetOrNew<ResLoadTask>();
            task.Add(new ResHandler(resPath, type));
            task.Start();
            return task;
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            resPath = InnerCheckName(resPath);
            ResLoadTask<T> task = Global.Task.GetOrNew<ResLoadTask<T>>();
            task.Add(new ResHandler(resPath, typeof(T)));
            task.Start();
            return task;
        }

        public void Unload(object res)
        {
            Resources.UnloadAsset((UnityEngine.Object)res);
        }

        public void UnloadAll()
        {
            Resources.UnloadUnusedAssets();
        }

        private string InnerCheckName(string resPath)
        {
            resPath = resPath.Replace(m_AssetPath, string.Empty);
            string extension = Path.GetExtension(resPath);
            if (!string.IsNullOrEmpty(extension))
                resPath = resPath.Replace(extension, string.Empty);
            return resPath;
        }
    }
}

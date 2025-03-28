﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public partial class ResourcesHelper : IResourceHelper
    {
        private string m_AssetPath;
        private IResRedirectHelper m_DirectHelper;

        void IResourceHelper.OnInit(string rootPath)
        {
            m_AssetPath = "Assets/Resources/";
        }

        public void SetResDirectHelper(IResRedirectHelper helper)
        {
            m_DirectHelper = helper;
        }

        public object Load(string resPath, Type type)
        {
            resPath = InnerCheckResPath(resPath, type);
            return Resources.Load(resPath, type);
        }

        public T Load<T>(string resPath)
        {
            Type type = typeof(T);
            resPath = InnerCheckResPath(resPath, type);
            return (T)Load(resPath, type);
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            resPath = InnerCheckResPath(resPath, type);
            return new ResLoadTask(new ResHandler(resPath, type));
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            Type type = typeof(T);
            resPath = InnerCheckResPath(resPath, type);
            return new ResLoadTask<T>(new ResHandler(resPath, type));
        }

        public void Unload(object res)
        {
            Resources.UnloadAsset((UnityEngine.Object)res);
        }

        public void UnloadAll()
        {
            Resources.UnloadUnusedAssets();
        }

        public List<object> DumpAll()
        {
            throw new NotImplementedException();
        }

        private string InnerCheckResPath(string resPath, Type type)
        {
            resPath = resPath.Replace(m_AssetPath, string.Empty);
            string extension = Path.GetExtension(resPath);
            if (!string.IsNullOrEmpty(extension))
                resPath = resPath.Replace(extension, string.Empty);
            if (m_DirectHelper != null)
                resPath = m_DirectHelper.Redirect(resPath, type);
            return resPath;
        }
    }
}

using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;
using XFrame.Core;

namespace UnityXFrame.Core.Resource
{
    public partial class AssetBundleResHelper : IResourceHelper
    {
        private string m_AssetsPath;
        private AssetBundle m_Main;
        private AssetBundleManifest m_MainManifest;
        private Dictionary<string, BundleInfo> m_Bundles;
        private Dictionary<string, AssetBundle> m_BundlesMap;
        private FileLoadInfo m_FileMap;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="rootPath">资源根路径，一般为persistDataPath, assetsStreaming</param>
        void IResourceHelper.OnInit(string rootPath)
        {
            m_AssetsPath = Path.Combine(Application.persistentDataPath, ROOT_DIR);
            string mainFilePath = Path.Combine(m_AssetsPath, MAIN_FILE);
            if (File.Exists(mainFilePath))
            {
                m_Bundles = new Dictionary<string, BundleInfo>();
                m_BundlesMap = new Dictionary<string, AssetBundle>();
                m_Main = AssetBundle.LoadFromFile(mainFilePath);
                m_MainManifest = m_Main.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));

                string infoStr = File.ReadAllText(Path.Combine(m_AssetsPath, RES2AB_FILE));
                m_FileMap = JsonConvert.DeserializeObject<FileLoadInfo>(infoStr);
            }
            else
            {
                Log.Error("Resource", "Res is null.");
            }
        }

        public void SetResDirectHelper(IResRedirectHelper helper)
        {

        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="resPath">资源路径, 注意从Assets目录开始，如:Assets/Data/Test.png</param>
        /// <param name="type">资源类型，注意加载图像资源时尽量传入Sprite或者Texture的类型</param>
        /// <returns>加载到的资源，如果资源路径不正确将返回空</returns>
        public object Load(string resPath, Type type)
        {
            resPath = InnerCheckFileName(resPath);
            if (m_FileMap.FileToABMap.TryGetValue(resPath, out string abName))
            {
                BundleInfo info = InnerLoadBundle(abName);
                return info.Load(resPath, type);
            }
            else
                return default;
        }

        public T Load<T>(string resPath)
        {
            resPath = InnerCheckFileName(resPath);
            if (m_FileMap.FileToABMap.TryGetValue(resPath, out string abName))
            {
                BundleInfo info = InnerLoadBundle(abName);
                return (T)(object)info.Load(resPath, typeof(T));
            }
            else
                return default;
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            resPath = InnerCheckFileName(resPath);
            if (m_FileMap.FileToABMap.TryGetValue(resPath, out string abName))
            {
                BundleInfo info = InnerLoadBundle(abName);
                ResLoadTask task = Global.Task.GetOrNew<ResLoadTask>();
                task.Add(new ResHandler(info, resPath, type));
                task.Start();
                return task;
            }
            else
                return default;
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            resPath = InnerCheckFileName(resPath);
            if (m_FileMap.FileToABMap.TryGetValue(resPath, out string abName))
            {
                BundleInfo info = InnerLoadBundle(abName);
                ResLoadTask<T> task = Global.Task.GetOrNew<ResLoadTask<T>>();
                task.Add(new ResHandler(info, resPath, typeof(T)));
                task.Start();
                return task;
            }
            else
                return default;
        }

        public void Unload(object package)
        {
            if (m_Bundles.TryGetValue((string)package, out BundleInfo bundleInfo))
                bundleInfo.Unload();
        }

        public void UnloadAll()
        {
            AssetBundle.UnloadAllAssetBundles(true);
        }

        #region Inner Implement
        private BundleInfo InnerLoadBundle(string abName)
        {
            if (m_Bundles.TryGetValue(abName, out BundleInfo info))
            {
                return info;
            }
            else
            {
                string path = Path.Combine(m_AssetsPath, abName);
                AssetBundle ab = AssetBundle.LoadFromFile(path);
                info = new BundleInfo(abName);

                string[] dpNames = m_MainManifest.GetAllDependencies(abName);
                BundleInfo[] dps = new BundleInfo[dpNames.Length];
                for (int i = 0; i < dpNames.Length; i++)
                {
                    string dpName = dpNames[i];
                    dps[i] = InnerLoadBundle(dpName);
                }

                info.Bundle = ab;
                info.Dependencies = dps;
                m_Bundles[abName] = info;
                return info;
            }
        }

        private string InnerCheckFileName(string path)
        {
            path = path.Replace('_', '/');
            path = path.Replace('\\', '/');
            return path.ToLower();
        }
        #endregion
    }
}

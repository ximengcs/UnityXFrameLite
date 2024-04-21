using System;
using XFrame.Core;
using UnityEngine;
using XFrame.Collections;
using XFrame.Modules.Resource;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using UnityEngine.U2D;
using XFrame.Utility;

namespace UnityXFrame.Core.Resource
{
    [CommonModule]
    [XType(typeof(ISpriteAtlasModule))]
    public partial class SpriteAtlasModule : ModuleBase, ISpriteAtlasModule, IResourceHelper
    {
        public const string ITEM_SPLIT = " ";
        private Type m_SpriteType;
        private Dictionary<string, string> m_ObjectMap;
        private IResModule m_ResModule;
        private IResRedirectHelper m_DirectHelper;

        public IResourceHelper Helper => m_DirectHelper;

        public string[] AllEntry
        {
            get
            {
                string[] entries = new string[m_ObjectMap.Count];
                int index = 0;
                foreach (var key in m_ObjectMap.Keys)
                    entries[index++] = key;
                return entries;
            }
        }

        void IResourceHelper.OnInit(string rootPath)
        {

        }

        protected override void OnStart()
        {
            base.OnStart();
            m_SpriteType = typeof(Sprite);
            m_ObjectMap = new Dictionary<string, string>();
            m_ResModule = Domain.GetModule<IResModule>(Id);
            m_ResModule.Helper.SetResDirectHelper(this);
        }

        public void PreloadAsset()
        {

        }

        public void UnloadAsset()
        {

        }

        public void AddEntry(string entryText)
        {
            string[] lines = entryText.Split('\n');
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                string[] items = line.Split(ITEM_SPLIT);
                if (items.Length < 2) continue;
                string spritePath = items[0];
                string atlasPath = PathUtility.RemoveEnterChar(items[1]);
                if (m_ObjectMap.ContainsKey(spritePath))
                {
                    Log.Debug("XFrame", $"sprite atlas module duplicate {spritePath}");
                }
                else
                {
                    m_ObjectMap.Add(spritePath, atlasPath);
                }
            }
        }

        public bool CanRedirect(string assetPath, Type assetType)
        {
            if (m_DirectHelper != null)
                assetPath = m_DirectHelper.Redirect(assetPath, assetType);
            if (assetType == m_SpriteType)
                return m_ObjectMap.ContainsKey(assetPath);
            else
                return false;
        }

        public bool Redirect(string assetPath, Type assetType, out string newAssetPath)
        {
            if (m_DirectHelper != null)
                assetPath = m_DirectHelper.Redirect(assetPath, assetType);
            if (assetType == m_SpriteType)
            {
                if (m_ObjectMap.TryGetValue(assetPath, out newAssetPath))
                {
                    return true;
                }
                else
                {
                    newAssetPath = assetPath;
                    return false;
                }
            }
            else
            {
                newAssetPath = assetPath;
                return false;
            }
        }

        public string Redirect(string assetPath, Type assetType)
        {
            if (m_DirectHelper != null)
                assetPath = m_DirectHelper.Redirect(assetPath, assetType);
            if (assetType == m_SpriteType)
            {
                if (m_ObjectMap.TryGetValue(assetPath, out string newAssetPath))
                {
                    Log.Debug("XFrame", $"Sprite Redirect To Atlas, {assetPath} -> {newAssetPath}");
                    return newAssetPath;
                }
                else
                {
                    return assetPath;
                }
            }
            else
            {
                return assetPath;
            }
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
            string atlasPath = Redirect(resPath, type);
            SpriteAtlas atlas = m_ResModule.Load<SpriteAtlas>(atlasPath);
            string spriteName = System.IO.Path.GetFileNameWithoutExtension(resPath);
            return atlas.GetSprite(spriteName);
        }

        public T Load<T>(string resPath)
        {
            string atlasPath = Redirect(resPath, typeof(T));
            SpriteAtlas atlas = m_ResModule.Load<SpriteAtlas>(atlasPath);
            string spriteName = System.IO.Path.GetFileNameWithoutExtension(resPath);
            object obj = atlas.GetSprite(spriteName);
            if (obj != null)
                return (T)obj;
            else
                return default;
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            string atlasPath = Redirect(resPath, type);
            ResLoadTask<SpriteAtlas> atlasTask = m_ResModule.LoadAsync<SpriteAtlas>(atlasPath);
            atlasTask.Coroutine();
            string spriteName = System.IO.Path.GetFileNameWithoutExtension(resPath);
            return new ResLoadTask(new WaitAtlasHandler(atlasTask, spriteName, type));
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            string atlasPath = Redirect(resPath, typeof(T));
            ResLoadTask<SpriteAtlas> atlasTask = m_ResModule.LoadAsync<SpriteAtlas>(atlasPath);
            atlasTask.Coroutine();
            string spriteName = System.IO.Path.GetFileNameWithoutExtension(resPath);
            return new ResLoadTask<T>(new WaitAtlasHandler(atlasTask, spriteName, typeof(T)));
        }

        public void Unload(object target)
        {
            m_ResModule.Unload(target);
        }

        public void UnloadAll()
        {
            m_ResModule.UnloadAll();
        }

        public List<object> DumpAll()
        {
            return m_ResModule.Helper.DumpAll();
        }
    }
}

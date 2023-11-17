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
    public partial class SpriteAtlasModule : ModuleBase, ISpriteAtlasModule
    {
        public const string ITEM_SPLIT = " ";
        private Type m_SpriteType;
        private Dictionary<string, string> m_ObjectMap;

        public IResourceHelper Helper => throw new NotImplementedException();

        protected override void OnStart()
        {
            base.OnStart();
            m_SpriteType = typeof(Sprite);
            m_ObjectMap = new Dictionary<string, string>();
            Global.Res.Helper.SetResDirectHelper(this);
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
            if (assetType == m_SpriteType)
                return m_ObjectMap.ContainsKey(assetPath);
            else
                return false;
        }

        public bool Redirect(string assetPath, Type assetType, out string newAssetPath)
        {
            if (assetType == m_SpriteType)
            {
                if (m_ObjectMap.TryGetValue(assetPath, out newAssetPath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                newAssetPath = null;
                return false;
            }
        }

        public string Redirect(string assetPath, Type assetType)
        {
            if (assetType == m_SpriteType)
            {
                if (m_ObjectMap.TryGetValue(assetPath, out string newAssetPath))
                {
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

        void IResourceHelper.OnInit(string rootPath)
        {

        }

        public void SetResDirectHelper(IResRedirectHelper helper)
        {

        }

        public object Load(string resPath, Type type)
        {
            string atlasPath = Redirect(resPath, type);
            SpriteAtlas atlas = Global.Res.Load<SpriteAtlas>(atlasPath);
            string spriteName = System.IO.Path.GetFileNameWithoutExtension(resPath);
            return atlas.GetSprite(spriteName);
        }

        public T Load<T>(string resPath)
        {
            string atlasPath = Redirect(resPath, typeof(T));
            SpriteAtlas atlas = Global.Res.Load<SpriteAtlas>(atlasPath);
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
            ResLoadTask loadTask = Global.Task.GetOrNew<ResLoadTask>();
            ResLoadTask<SpriteAtlas> atlasTask = Global.Res.LoadAsync<SpriteAtlas>(atlasPath);
            string spriteName = System.IO.Path.GetFileNameWithoutExtension(resPath);
            loadTask.Add(new WaitAtlasHandler(atlasTask, spriteName));
            return loadTask;
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            string atlasPath = Redirect(resPath, typeof(T));
            ResLoadTask<T> loadTask = Global.Task.GetOrNew<ResLoadTask<T>>();
            ResLoadTask<SpriteAtlas> atlasTask = Global.Res.LoadAsync<SpriteAtlas>(atlasPath);
            string spriteName = System.IO.Path.GetFileNameWithoutExtension(resPath);
            loadTask.Add(new WaitAtlasHandler(atlasTask, spriteName));
            return loadTask;
        }

        public void Unload(object target)
        {
            Global.Res.Unload(target);
        }

        public void UnloadAll()
        {
            Global.Res.UnloadAll();
        }
    }
}

using System;
using XFrame.Utility;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public class ResourceAlaisRedirector : IResRedirectHelper
    {
        public const string ITEM_SPLIT = " ";
        private HashSet<int> m_CurrentGroup;
        private IResourceHelper m_ResRefHelper;
        private Dictionary<int, Dictionary<string, string>> m_AliasMap;

        public IEnumerable<int> Group => m_CurrentGroup;

        public ResourceAlaisRedirector(IResourceHelper resHelper)
        {
            m_ResRefHelper = resHelper;
            m_AliasMap = new Dictionary<int, Dictionary<string, string>>();
            m_CurrentGroup = new HashSet<int>();
        }

        void IResourceHelper.OnInit(string rootPath)
        {

        }

        public void ClearGroup()
        {
            m_CurrentGroup.Clear();
        }

        public void AddGroup(int group)
        {
            if (!m_CurrentGroup.Contains(group))
                m_CurrentGroup.Add(group);
        }

        public void RemoveGroup(int group)
        {
            if (m_CurrentGroup.Contains(group))
                m_CurrentGroup.Remove(group);
        }

        public void AddEntry(string entryItems)
        {
            int groupId = -1;
            string[] lines = entryItems.Split('\n');
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                string[] items = line.Split(ITEM_SPLIT);
                if (items.Length == 1)
                {
                    if (!int.TryParse(items[0], out groupId))
                        groupId = -1;
                    continue;
                }
                string assetPath = items[0];
                string aliasPath = PathUtility.RemoveEnterChar(items[1]);

                if (!m_AliasMap.TryGetValue(groupId, out Dictionary<string, string> map))
                {
                    map = new Dictionary<string, string>();
                    m_AliasMap[groupId] = map;
                }
                if (map.ContainsKey(assetPath))
                {
                    Log.Debug("XFrame", $"res alias name duplicate {assetPath}");
                }
                else
                {
                    map.Add(assetPath, aliasPath);
                }
            }
        }

        public void AddEntry(int group, string entryItems)
        {
            if (!m_AliasMap.TryGetValue(group, out Dictionary<string, string> map))
            {
                map = new Dictionary<string, string>();
                m_AliasMap[group] = map;
            }

            string[] lines = entryItems.Split('\n');
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                string[] items = line.Split(ITEM_SPLIT);
                if (items.Length < 2) continue;
                string assetPath = items[0];
                string aliasPath = PathUtility.RemoveEnterChar(items[1]);
                if (map.ContainsKey(assetPath))
                {
                    Log.Debug("XFrame", $"res alias name duplicate {assetPath}");
                }
                else
                {
                    map.Add(assetPath, aliasPath);
                }
            }
        }

        bool IResRedirectHelper.CanRedirect(string assetPath, Type assetType)
        {
            foreach (int group in m_CurrentGroup)
            {
                if (m_AliasMap.TryGetValue(group, out Dictionary<string, string> map))
                {
                    if (map.ContainsKey(assetPath))
                        return true;
                }
            }

            return false;
        }

        bool IResRedirectHelper.Redirect(string assetPath, Type assetType, out string newAssetPath)
        {
            foreach (int group in m_CurrentGroup)
            {
                if (m_AliasMap.TryGetValue(group, out Dictionary<string, string> map))
                {
                    if (map.TryGetValue(assetPath, out newAssetPath))
                    {
                        return true;
                    }
                }
            }

            newAssetPath = assetPath;
            return false;
        }

        public string Redirect(string assetPath, Type assetType)
        {
            foreach (int group in m_CurrentGroup)
            {
                if (m_AliasMap.TryGetValue(group, out Dictionary<string, string> map))
                {
                    if (map.TryGetValue(assetPath, out string newAssetPath))
                    {
                        return newAssetPath;
                    }
                }
            }

            return assetPath;
        }

        void IResourceHelper.SetResDirectHelper(IResRedirectHelper helper)
        {

        }


        object IResourceHelper.Load(string resPath, Type type)
        {
            resPath = Redirect(resPath, type);
            return m_ResRefHelper.Load(resPath, type);
        }

        T IResourceHelper.Load<T>(string resPath)
        {
            resPath = Redirect(resPath, typeof(T));
            return m_ResRefHelper.Load<T>(resPath);
        }

        ResLoadTask IResourceHelper.LoadAsync(string resPath, Type type)
        {
            resPath = Redirect(resPath, type);
            return m_ResRefHelper.LoadAsync(resPath, type);
        }

        ResLoadTask<T> IResourceHelper.LoadAsync<T>(string resPath)
        {
            resPath = Redirect(resPath, typeof(T));
            return m_ResRefHelper.LoadAsync<T>(resPath);
        }

        void IResourceHelper.Unload(object target)
        {
            m_ResRefHelper.Unload(target);
        }

        void IResourceHelper.UnloadAll()
        {
            m_ResRefHelper.UnloadAll();
        }
    }
}

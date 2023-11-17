using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;
using System.IO;
using XFrame.Utility;
using UnityXFrame.Core;
using System.Text;
using UnityXFrame.Core.Resource;

namespace UnityXFrame.Editor
{
    public class AtlasResEditor : EditorWindow
    {
        private Dictionary<string, string> m_Maps;
        private string m_SavePath;

        private void OnEnable()
        {
            m_Maps = new Dictionary<string, string>();
            m_SavePath = Constant.CONFIG_PATH;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Collect Map"))
            {
                m_Maps.Clear();
                InnerFindAllAtlas();
                InnerSaveFile();
            }
        }

        private void InnerSaveFile()
        {
            string fileName = Path.Combine(Application.dataPath, m_SavePath, "atlas_map.txt");
            if (!Directory.Exists(m_SavePath))
                Directory.CreateDirectory(m_SavePath);
            if (File.Exists(fileName))
                File.Delete(fileName);
            StringBuilder sb = new StringBuilder();
            foreach (var map in m_Maps)
            {
                sb.AppendLine($"{map.Key}{SpriteAtlasModule.ITEM_SPLIT}{map.Value}");
            }
            File.WriteAllText(fileName, sb.ToString());
            EditorLog.Debug($"save -> {fileName}");
            AssetDatabase.Refresh();
        }

        private void InnerFindAllAtlas()
        {
            string[] reses = AssetDatabase.FindAssets("t:spriteatlas");
            for (int i = 0; i < reses.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(reses[i]);
                SpriteAtlas assets = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);

                if (assets == null)
                    continue;

                InnerSaveAtlas(assets);
            }
        }

        private void InnerSaveAtlas(SpriteAtlas atlas)
        {
            Object[] objects = SpriteAtlasExtensions.GetPackables(atlas);
            foreach (Object obj in objects)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                if (AssetDatabase.IsValidFolder(assetPath))
                {
                    List<string> spriteList = new List<string>();
                    InnerFindAllSprite(assetPath, spriteList);
                    foreach (string spriteAssetPath in spriteList)
                        InnnerSaveObject(atlas, spriteAssetPath);
                }
                else
                {
                    InnnerSaveObject(atlas, assetPath);
                }
            }
        }

        private void InnerFindAllSprite(string path, List<string> result)
        {
            foreach (string file in Directory.EnumerateFiles(path))
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(file);
                if (sprite != null)
                {
                    string resultPath = PathUtility.Format1(file);
                    result.Add(resultPath);
                }
            }

            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                InnerFindAllSprite(folder, result);
            }
        }

        private void InnnerSaveObject(SpriteAtlas atlas, string assetPath)
        {
            string atlasPath = AssetDatabase.GetAssetPath(atlas);
            if (IsAssetAddressable(atlasPath, out string path))
            {
                atlasPath = path;
            }

            m_Maps.Add(assetPath, atlasPath);
            EditorLog.Debug($"{assetPath} -> {atlasPath}");
        }

        private bool IsAssetAddressable(string assetPath, out string path)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetEntry entry = settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(assetPath), true);
            if (entry != null)
            {
                path = InnerGetAddress(entry.ParentEntry);
                if (string.IsNullOrEmpty(path))
                    path = Path.GetFileName(entry.AssetPath);
                else
                    path = $"{path}/{Path.GetFileName(entry.AssetPath)}";
            }
            else
                path = null;
            return entry != null;
        }

        private string InnerGetAddress(AddressableAssetEntry entry)
        {
            return entry != null ? $"{(entry.ParentEntry != null ? InnerGetAddress(entry.ParentEntry) + "/" : "")}{entry.address}" : "";
        }
    }
}

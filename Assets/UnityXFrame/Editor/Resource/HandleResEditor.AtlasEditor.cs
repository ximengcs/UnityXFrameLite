
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using UnityXFrame.Core.Resource;
using XFrame.Modules.Reflection;
using XFrame.Utility;

namespace UnityXFrame.Editor
{
    public partial class HandleResEditor
    {
        private Dictionary<string, string> m_Maps;
        private HashSet<string> m_DuplicateCheck;
        private SerializedProperty m_ListProp;

        private void InnerInitAtlas()
        {
            m_Maps = new Dictionary<string, string>();
            m_DuplicateCheck = new HashSet<string>();
            m_ListProp = m_Obj.FindProperty("ExcludeList");
        }

        private void InnerRenderAtlasGUI()
        {
            EditorGUILayout.PropertyField(m_ListProp);
            if (GUILayout.Button("Collect Atlas Map"))
            {
                m_Maps.Clear();
                m_DuplicateCheck.Clear();
                InnerFindAllAtlas();
                InnerSaveFile();
            }
        }

        private void InnerFindAllAtlas()
        {
            string[] reses = AssetDatabase.FindAssets("t:spriteatlas");
            for (int i = 0; i < reses.Length; i++)
            {
                string name = reses[i];
                string path = AssetDatabase.GUIDToAssetPath(name);
                if (m_Data.ExcludeList == null)
                    m_Data.ExcludeList = new List<string>();
                bool next = false;
                foreach (string str in m_Data.ExcludeList)
                {
                    if (path.Contains(str))
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                    continue;

                SpriteAtlas assets = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);

                if (assets == null)
                    continue;

                InnerSaveAtlas(assets);
            }
        }

        private void InnerSaveAtlas(SpriteAtlas atlas)
        {
            UnityEngine.Object[] objects = SpriteAtlasExtensions.GetPackables(atlas);
            foreach (UnityEngine.Object obj in objects)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                if (!m_DuplicateCheck.Contains(assetPath))
                {
                    m_DuplicateCheck.Add(assetPath);
                }
                else
                {
                    EditorLog.Error($"Duplicate {atlas.name} {assetPath} {atlas.name}");
                }

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
                string resPath = map.Key;
                string atlasPath = map.Value;
                if (m_Handler != null)
                {
                    m_Handler.OnSaveAtlasPath(resPath, atlasPath, out resPath, out atlasPath);
                }
                sb.AppendLine($"{resPath}{SpriteAtlasModule.ITEM_SPLIT}{atlasPath}");
            }
            File.WriteAllText(fileName, sb.ToString());
            EditorLog.Debug($"save -> {fileName}");
            AssetDatabase.Refresh();
        }

        private void InnnerSaveObject(SpriteAtlas atlas, string assetPath)
        {
            string atlasPath = AssetDatabase.GetAssetPath(atlas);
            if (IsAssetAddressable(atlasPath, out string path))
            {
                atlasPath = path;
            }
            if (!m_Maps.ContainsKey(assetPath))
            {
                m_Maps.Add(assetPath, atlasPath);
                EditorLog.Debug($"{assetPath} -> {atlasPath}");
            }
            else
            {
                EditorLog.Error($"{assetPath} -> {atlasPath}");
            }
        }

    }
}

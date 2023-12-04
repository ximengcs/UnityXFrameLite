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
using System;
using XFrame.Modules.Reflection;
using XFrame.Core;
using XFrame.Modules.Config;

namespace UnityXFrame.Editor
{
    public partial class AtlasResEditor : EditorWindow
    {
        private Dictionary<string, string> m_Maps;
        private string m_SavePath;
        private HashSet<string> m_DuplicateCheck;
        private AtlasResData m_Data;
        private SerializedObject m_Obj;
        private SerializedProperty m_ListProp;
        private XCore m_FrameCore;

        private void OnEnable()
        {
            string dataPath = "Assets/UnityXFrame/Editor/AtlasResData.asset";
            m_Data = AssetDatabase.LoadAssetAtPath<AtlasResData>(dataPath);
            if (m_Data == null)
            {
                m_Data = ScriptableObject.CreateInstance<AtlasResData>();
                AssetDatabase.CreateAsset(m_Data, dataPath);
                AssetDatabase.Refresh();
            }
            m_Maps = new Dictionary<string, string>();
            m_DuplicateCheck = new HashSet<string>();
            m_SavePath = Constant.CONFIG_PATH;

            m_Obj = new SerializedObject(m_Data);
            m_ListProp = m_Obj.FindProperty("ExcludeList");

            XConfig.UseClassModule = new string[] { "Assembly-CSharp", "Assembly-CSharp-Editor", "UnityXFrame", "UnityXFrame.Lib", "UnityXFrame.Editor" };
            m_FrameCore = XCore.Create(typeof(TypeModule));
        }

        private void OnGUI()
        {
            EditorGUILayout.PropertyField(m_ListProp);
            if (GUILayout.Button("Collect Map"))
            {
                m_Maps.Clear();
                m_DuplicateCheck.Clear();
                InnerFindAllAtlas();
                InnerSaveFile();
            }
            if (GUILayout.Button("Save"))
                m_Obj.ApplyModifiedProperties();
        }

        private void InnerSaveFile()
        {
            string fileName = Path.Combine(Application.dataPath, m_SavePath, "atlas_map.txt");
            if (!Directory.Exists(m_SavePath))
                Directory.CreateDirectory(m_SavePath);
            if (File.Exists(fileName))
                File.Delete(fileName);

            ITypeModule typeModule = m_FrameCore.GetModule<ITypeModule>();
            TypeSystem typeSystem = typeModule.GetOrNew<IAtlasResHandler>();
            IAtlasResHandler handler = null;
            foreach (Type type in typeSystem)
            {
                handler = (IAtlasResHandler)typeModule.CreateInstance(type);
                break;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var map in m_Maps)
            {
                string resPath = map.Key;
                string atlasPath = map.Value;
                if (handler != null)
                {
                    handler.OnSavePath(resPath, atlasPath, out resPath, out atlasPath);
                }
                sb.AppendLine($"{resPath}{SpriteAtlasModule.ITEM_SPLIT}{atlasPath}");
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

                EditorLog.Debug($"Name {path}");
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
                EditorLog.Debug($"{assetPath} -> {atlasPath} -> {atlas.name}");
            }
            else
            {
                EditorLog.Error($"{assetPath} -> {atlasPath} -> {atlas.name}");
            }
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

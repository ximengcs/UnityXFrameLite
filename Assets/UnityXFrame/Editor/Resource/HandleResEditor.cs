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
using UnityEngine.AddressableAssets;

namespace UnityXFrame.Editor
{
    public partial class HandleResEditor : EditorWindow
    {
        private Dictionary<string, string> m_Maps;
        private string m_SavePath;
        private HashSet<string> m_DuplicateCheck;
        private HandleResData m_Data;
        private SerializedObject m_Obj;
        private SerializedProperty m_ListProp;
        private SerializedProperty m_AliasListProp;
        private XCore m_FrameCore;

        private Dictionary<int, List<string>> m_FileList;

        private void OnEnable()
        {
            string dataPath = "Assets/UnityXFrame/Editor/HandleResData.asset";
            m_Data = AssetDatabase.LoadAssetAtPath<HandleResData>(dataPath);
            if (m_Data == null)
            {
                m_Data = ScriptableObject.CreateInstance<HandleResData>();
                AssetDatabase.CreateAsset(m_Data, dataPath);
                AssetDatabase.Refresh();
            }
            m_FileList = new Dictionary<int, List<string>>();
            m_Maps = new Dictionary<string, string>();
            m_DuplicateCheck = new HashSet<string>();
            m_SavePath = Constant.CONFIG_PATH;

            m_Obj = new SerializedObject(m_Data);
            m_ListProp = m_Obj.FindProperty("ExcludeList");
            m_AliasListProp = m_Obj.FindProperty("IncludeGroupList");

            XConfig.UseClassModule = new string[] { "Assembly-CSharp", "Assembly-CSharp-Editor", "UnityXFrame", "UnityXFrame.Lib", "UnityXFrame.Editor" };
            m_FrameCore = XCore.Create(typeof(TypeModule));
        }

        private void OnGUI()
        {
            EditorGUILayout.PropertyField(m_ListProp);
            if (GUILayout.Button("Collect Atlas Map"))
            {
                m_Maps.Clear();
                m_DuplicateCheck.Clear();
                InnerFindAllAtlas();
                InnerSaveFile();
            }
            EditorGUILayout.PropertyField(m_AliasListProp);
            if (GUILayout.Button("Collect Res Alias"))
            {
                m_FileList.Clear();
                InnerFindAllFile();
                InnerSaveAliasFile();
            }
            if (GUILayout.Button("Save Data"))
                m_Obj.ApplyModifiedProperties();
        }

        private void InnerSaveAliasFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entry in m_FileList)
            {
                sb.AppendLine(entry.Key.ToString());
                foreach (string file in entry.Value)
                {
                    string fileName = Path.GetFileName(file);
                    sb.AppendLine($"{fileName} {file}");
                }
            }

            string tarFile = Path.Combine(Application.dataPath, m_SavePath, "alias_map.txt");
            if (!Directory.Exists(m_SavePath))
                Directory.CreateDirectory(m_SavePath);
            if (File.Exists(tarFile))
                File.Delete(tarFile);

            File.WriteAllText(tarFile, sb.ToString());
            EditorLog.Debug($"save -> {tarFile}");
            AssetDatabase.Refresh();
        }

        private void InnerFindAllFile()
        {
            foreach (ResAliasGroupData groupData in m_Data.IncludeGroupList)
            {
                List<string> result = new List<string>();
                HashSet<string> checker = new HashSet<string>();
                foreach (string path in groupData.IncludeList)
                {
                    InnerFindPath(path, result, checker);
                }
                EditorLog.Debug($"Add Group {groupData.GroupId}, amount {result.Count}");
                m_FileList.Add(groupData.GroupId, result);
            }
        }

        private void InnerFindPath(string path, List<string> result, HashSet<string> checker)
        {
            foreach (string _file in Directory.EnumerateFiles(path))
            {
                if (_file.Contains(".meta"))
                    continue;
                string file = PathUtility.Format1(_file);
                string target;
                if (IsAssetAddressable(file, out string newPath))
                {
                    EditorLog.Debug($"NewPath {file} -> {newPath}");
                    target = newPath;
                }
                else
                {
                    target = file;
                }

                result.Add(target);
                if (!checker.Contains(target))
                    checker.Add(target);
                else
                    EditorLog.Error($"Duplicate {target} -> {file}");
            }
            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                InnerFindPath(dir, result, checker);
            }
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
                path = entry.address;
                if (entry.ParentEntry != null)
                {
                    if (path.StartsWith(entry.ParentEntry.address))
                    {
                        path = path.Replace(entry.ParentEntry.address, $"{entry.ParentEntry.address}/");
                    }
                }
                //path = InnerGetAddress(entry.ParentEntry);
                if (string.IsNullOrEmpty(path))
                    path = Path.GetFileName(entry.AssetPath);
                //else
                //    path = $"{path}/{Path.GetFileName(entry.AssetPath)}";
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

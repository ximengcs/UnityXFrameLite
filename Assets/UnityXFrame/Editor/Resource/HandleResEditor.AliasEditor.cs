
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core.Resource;
using XFrame.Utility;

namespace UnityXFrame.Editor
{
    public partial class HandleResEditor
    {
        private bool m_ContainsSuffix;
        private SerializedProperty m_AliasListProp;
        private Dictionary<int, List<string>> m_FileList;

        private void InnerInitAlias()
        {
            m_AliasListProp = m_Obj.FindProperty("IncludeGroupList");
            m_FileList = new Dictionary<int, List<string>>();
        }

        private void InnerRenderAliasGUI()
        {
            EditorGUILayout.PropertyField(m_AliasListProp);
            if (GUILayout.Button("Collect Res Alias"))
            {
                m_FileList.Clear();
                InnerFindAllFile();
                InnerSaveAliasFile();
            }
            m_ContainsSuffix = EditorGUILayout.Toggle("ContainsSuffix", m_ContainsSuffix);
        }


        private void InnerFindPath(string path, List<string> result, HashSet<string> checker, string rootPath)
        {
            foreach (string _file in Directory.EnumerateFiles(path))
            {
                if (_file.Contains(".meta"))
                    continue;
                string file = PathUtility.Format1(_file);
                string target;
                if (IsAssetAddressable(file, out string newPath))
                {
                    target = newPath;
                }
                else
                {
                    target = file;
                }

                if (m_Handler != null)
                    m_Handler.OnSaveAliasPath(rootPath, target, out target);
                result.Add(target);
                if (!checker.Contains(target))
                    checker.Add(target);
                else
                    EditorLog.Error($"Duplicate {target} -> {file}");
            }
            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                InnerFindPath(dir, result, checker, rootPath);
            }
        }

        private void InnerFindAllFile()
        {
            foreach (ResAliasGroupData groupData in m_Data.IncludeGroupList)
            {
                List<string> result = new List<string>();
                HashSet<string> checker = new HashSet<string>();
                foreach (string path in groupData.IncludeList)
                {
                    InnerFindPath(path, result, checker, path);
                }
                EditorLog.Debug($"Add Group {groupData.GroupId}, amount {result.Count}");

                m_FileList.Add(groupData.GroupId, result);
            }
        }

        private void InnerSaveAliasFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entry in m_FileList)
            {
                sb.AppendLine(entry.Key.ToString());
                foreach (string file in entry.Value)
                {
                    string fileName;
                    if (m_ContainsSuffix)
                        fileName= Path.GetFileName(file);
                    else
                        fileName = Path.GetFileNameWithoutExtension(file);
                    sb.AppendLine($"{fileName}{ResourceAlaisRedirector.ITEM_SPLIT}{file}");
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
    }
}

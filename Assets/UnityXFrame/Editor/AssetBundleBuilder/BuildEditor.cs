using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using XFrame.Modules.Archives;
using UnityXFrame.Core.Resource;

namespace UnityXFrame.Editor
{
    public partial class BuildEditor : EditorWindow
    {
        private bool m_IsRelease;
        private bool m_DeleteTmp;
        private BuildTarget m_Platform;
        private string m_OutPath;

        private void OnEnable()
        {
            m_IsRelease = false;
            m_DeleteTmp = true;

            m_OutPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\Bundle2";
        }

        private void OnGUI()
        {
            m_OutPath = EditorGUILayout.TextField(m_OutPath);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Release");
            m_IsRelease = EditorGUILayout.Toggle(m_IsRelease);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Delete Temp");
            m_DeleteTmp = EditorGUILayout.Toggle(m_DeleteTmp);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Platform");
            m_Platform = (BuildTarget)EditorGUILayout.EnumPopup(m_Platform);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Build"))
            {
                InnerBuild();
                InnerCheckVersion();
            }
        }

        private void InnerBuild()
        {
            BuildTarget target = default;
            switch (m_Platform)
            {
                case BuildTarget.StandaloneWindows: target = BuildTarget.StandaloneWindows; break;
                case BuildTarget.Android: target = BuildTarget.Android; break;
                case BuildTarget.iOS: target = BuildTarget.iOS; break;
                default: return;
            }

            string version = PlayerSettings.bundleVersion;
            string basePath = Path.Combine(m_OutPath, m_Platform.ToString());
            string outpath = Path.Combine(basePath, version);
            if (!Directory.Exists(outpath))
                Directory.CreateDirectory(outpath);

            foreach (string file in Directory.EnumerateFiles(outpath))
                File.Delete(file);

            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(outpath, BuildAssetBundleOptions.None, target);

            foreach (string file in Directory.EnumerateFiles(outpath))
            {
                string prePath = Path.GetDirectoryName(file);
                string fileName = Path.GetFileName(file);
                if (fileName.Contains(version))
                {
                    string targetName = Path.Combine(prePath, fileName.Replace(version, "bundles"));
                    File.Move(file, targetName);
                }
            }

            AssetBundleResHelper.FileLoadInfo info = new AssetBundleResHelper.FileLoadInfo();
            info.MainName = "bundles.manifest";
            foreach (string abName in manifest.GetAllAssetBundles())
            {
                string abPath = Path.Combine(outpath, abName);
                AssetBundle ab = AssetBundle.LoadFromFile(abPath);
                foreach (string containFile in ab.GetAllAssetNames())
                    info.Add(containFile, abName);
            }

            string fileInfoJson = JsonConvert.SerializeObject(info, Formatting.Indented);
            string fileInfoJsonPath = Path.Combine(outpath, $"{info.MainName}.fi");
            if (File.Exists(fileInfoJsonPath))
                File.Delete(fileInfoJsonPath);
            File.WriteAllText(fileInfoJsonPath, fileInfoJson);

            DataArchive archive = DataArchive.LoadPath(outpath, Path.Combine(outpath, "res.ab"));
            if (m_DeleteTmp)
            {
                foreach (string file in Directory.EnumerateFiles(outpath))
                {
                    File.Delete(file);
                }
            }

            archive.Save();

            //VersionInfo vInfo = new VersionInfo();
            //vInfo.Version = version;
            //string json = JsonConvert.SerializeObject(vInfo, Formatting.Indented);
            //string vPath = Path.Combine(basePath, "version.info");
            //if (File.Exists(vPath))
            //    File.Delete(vPath);
            //File.WriteAllText(vPath, json);

            AssetDatabase.Refresh();
        }

        private void InnerCheckVersion()
        {
            if (m_IsRelease)
            {
                string[] versionString = PlayerSettings.bundleVersion.Split('.');
                Version version = new Version(int.Parse(versionString[0]), int.Parse(versionString[1]) + 1);
                PlayerSettings.bundleVersion = version.ToString();
            }
        }
    }
}

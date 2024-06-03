using System.IO;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using UnityEditor.Build.Player;
using System.Collections.Generic;
using System;

namespace UnityXFrame.Editor
{
    public class UsefulToolEditor : EditorWindow
    {
        private UsefulToolData m_Data;
        [NonSerialized] private Vector2 m_Pos;

        private void OnEnable()
        {
            string dataPath = "Assets/UnityXFrame/Editor/UserfulToolData.asset";
            m_Data = AssetDatabase.LoadAssetAtPath<UsefulToolData>(dataPath);
            if (m_Data == null)
            {
                m_Data = ScriptableObject.CreateInstance<UsefulToolData>();
                AssetDatabase.CreateAsset(m_Data, dataPath);
                AssetDatabase.Refresh();
            }
            titleContent = new GUIContent("Tool");
        }

        private void OnGUI()
        {
            int select = m_Data.IsRelease ? 1 : 0;
            select = GUILayout.Toolbar(select, new string[] { "Debug", "Release" });
            m_Data.IsRelease = select == 1 ? true : false;

            GUILayout.BeginVertical(StyleUtility.Style1);
            EditorGUILayout.LabelField("XFrame", StyleUtility.Style2);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Project", GUILayout.Width(50));
            m_Data.XFrameProjectPath = EditorGUILayout.TextField(m_Data.XFrameProjectPath);
            if (GUILayout.Button("→", GUILayout.Width(20)))
                EditorUtility.RevealInFinder(XFrameDllPath());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Position", GUILayout.Width(50));
            m_Data.XFramePath = EditorGUILayout.TextField(m_Data.XFramePath);
            if (GUILayout.Button("→", GUILayout.Width(20)))
                EditorUtility.RevealInFinder(m_Data.XFramePath);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Compile"))
                CompileXFrame();
            if (GUILayout.Button("Import"))
                ImportXFrame();
            if (GUILayout.Button("Compile & Import"))
                CompileXFrame((s, e) => ImportXFrame());
            GUILayout.EndVertical();

            GUILayout.BeginVertical(StyleUtility.Style1);
            EditorGUILayout.LabelField("UnityXFrame", StyleUtility.Style2);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Platform", GUILayout.Width(80));
            m_Data.CurrentBuildTarget = (BuildTarget)EditorGUILayout.EnumPopup(m_Data.CurrentBuildTarget);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Compile To", GUILayout.Width(80)))
                CompileDll();
            m_Data.BuildDllPath = EditorGUILayout.TextField(m_Data.BuildDllPath);
            if (GUILayout.Button("→", GUILayout.Width(20)))
                EditorUtility.RevealInFinder(m_Data.BuildDllPath);
            EditorGUILayout.EndHorizontal();

            m_Pos = EditorGUILayout.BeginScrollView(m_Pos, StyleUtility.Style1, GUILayout.Height(100));
            List<int> removes = new List<int>();
            if (m_Data.ToProjectDllPath == null)
                m_Data.ToProjectDllPath = new List<string>();
            for (int i = 0; i < m_Data.ToProjectDllPath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy To", GUILayout.Width(80)))
                    CopyToProject(m_Data.ToProjectDllPath[i]);
                m_Data.ToProjectDllPath[i] = EditorGUILayout.TextField(m_Data.ToProjectDllPath[i]);
                if (GUILayout.Button("x", GUILayout.Width(20)))
                    removes.Add(i);
                if (GUILayout.Button("→", GUILayout.Width(20)))
                    EditorUtility.RevealInFinder(m_Data.ToProjectDllPath[i]);
                EditorGUILayout.EndHorizontal();
            }
            foreach (int i in removes)
                m_Data.ToProjectDllPath.RemoveAt(i);
            if (GUILayout.Button("+"))
                m_Data.ToProjectDllPath.Add(string.Empty);
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Compile & Copy"))
            {
                CompileDll();
                CopyToProject();
            }
            GUILayout.EndVertical();


            GUILayout.BeginVertical(StyleUtility.Style1);
            EditorGUILayout.LabelField("XFrame Share", StyleUtility.Style2);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Project", GUILayout.Width(50));
            m_Data.XFrameSharePath = EditorGUILayout.TextField(m_Data.XFrameSharePath);
            if (GUILayout.Button("→", GUILayout.Width(20)))
                EditorUtility.RevealInFinder(XFrameDllPath());
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Import"))
                ImportXFrameShareDll();
            GUILayout.EndVertical();

            EditorUtility.SetDirty(m_Data);
        }

        public void ImportXFrameShareDll()
        {
            if (EditorApplication.isCompiling)
            {
                EditorLog.Debug($"Script is compiling");
                return;
            }
            string path = XFrameShareDllPath();
            foreach (string p in Directory.EnumerateFiles(path))
            {
                if (Path.GetFileName(p).Contains("XFrameShare"))
                {
                    string toPath = Path.Combine(m_Data.XFramePath, Path.GetFileName(p));
                    byte[] data = File.ReadAllBytes(p);
                    File.WriteAllBytes(toPath, data);
                    EditorLog.Debug($"{p} {EditorLog.Color("->", Color.green)} {toPath}");
                }
            }

            AssetDatabase.Refresh();
        }

        public void DeleteXFrameCache()
        {
            string targetPath = Path.Combine(m_Data.XFrameProjectPath, "XFrame", "obj");
            if (Directory.Exists(targetPath))
                Directory.Delete(targetPath, true);
            EditorLog.Debug($"delete {targetPath}");
            targetPath = Path.Combine(m_Data.XFrameProjectPath, "XFrame", "bin");
            if (Directory.Exists(targetPath))
                Directory.Delete(targetPath, true);
            targetPath = XFrameDllPath();
            if (Directory.Exists(targetPath))
                Directory.Delete(targetPath, true);
            EditorLog.Debug($"delete {targetPath}");
        }

        public void CompileXFrame(EventHandler callback = null)
        {
            DeleteXFrameCache();
            EditorLog.Debug("========================== START COMPILE XFRAME ========================");
            string fullPath = Path.Combine(m_Data.XFrameProjectPath, "XFrame/XFrame.csproj");
            string param = "build ";
            if (m_Data.IsRelease)
                param += "--configuration Release ";
            param += fullPath;
            param += " -o " + XFrameDllPath();
            ProcessStartInfo startInfo = new ProcessStartInfo("dotnet", param);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = Process.Start(startInfo);
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += (sender, a) => EditorLog.Debug(a.Data);
            process.Exited += callback;
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            process.Close();
            EditorLog.Debug("=======================================================================");
        }

        public string XFrameDllPath()
        {
            return m_Data.XFrameProjectPath + "/Tmp";
        }

        public string XFrameShareDllPath()
        {
            return m_Data.XFrameSharePath + "/bin/Debug/netstandard2.1";
        }

        public string XFrameShareDllPath2()
        {
            return m_Data.XFrameSharePath + "/bin/Debug/net8.0";
        }

        public void ImportXFrame()
        {
            if (EditorApplication.isCompiling)
            {
                EditorLog.Debug($"Script is compiling");
                return;
            }
            string path = XFrameDllPath();
            foreach (string p in Directory.EnumerateFiles(path))
            {
                if (Path.GetFileName(p).Contains("XFrame"))
                {
                    string toPath = Path.Combine(m_Data.XFramePath, Path.GetFileName(p));
                    byte[] data = File.ReadAllBytes(p);
                    File.WriteAllBytes(toPath, data);
                    EditorLog.Debug($"{p} {EditorLog.Color("->", Color.green)} {toPath}");
                }
            }

            AssetDatabase.Refresh();
        }

        public void CompileDll()
        {
            if (EditorApplication.isCompiling)
            {
                EditorLog.Debug($"Script is compiling");
                return;
            }
            if (Directory.Exists(m_Data.BuildDllPath))
            {
                foreach (string file in Directory.EnumerateFiles(m_Data.BuildDllPath))
                    File.Delete(file);
            }
            else
            {
                Directory.CreateDirectory(m_Data.BuildDllPath);
            }

            var group = BuildPipeline.GetBuildTargetGroup(m_Data.CurrentBuildTarget);
            ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
            scriptCompilationSettings.group = group;
            scriptCompilationSettings.target = m_Data.CurrentBuildTarget;
            Directory.CreateDirectory(m_Data.BuildDllPath);
            PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, m_Data.BuildDllPath);
        }

        public void CopyToProject(string projectPath)
        {
            if (EditorApplication.isCompiling)
            {
                EditorLog.Debug($"Script is compiling");
                return;
            }
            if (!string.IsNullOrEmpty(m_Data.BuildDllPath))
            {
                foreach (string path in Directory.EnumerateFiles(m_Data.BuildDllPath))
                {
                    if (Path.GetFileName(path).Contains("UnityXFrame"))
                    {
                        string toPath = Path.Combine(projectPath, Path.GetFileName(path));
                        File.Copy(path, toPath, true);
                        EditorLog.Debug($"{path} {EditorLog.Color("->", Color.green)} {toPath}");
                    }
                }
            }

            string assemblyPath = Application.dataPath + "/../Library/ScriptAssemblies/";
            foreach (string path in Directory.EnumerateFiles(assemblyPath))
            {
                if (Path.GetFileName(path).Contains("UnityXFrame.Editor"))
                {
                    string toPathDir = Path.Combine(projectPath, "Editor");
                    string toPath = Path.Combine(toPathDir, Path.GetFileName(path));
                    if (!Directory.Exists(toPathDir))
                        Directory.CreateDirectory(toPathDir);
                    File.Copy(path, toPath, true);
                    EditorLog.Debug($"{path} {EditorLog.Color("->", Color.green)} {toPath}");
                }
            }

            foreach (string path in Directory.EnumerateFiles(m_Data.XFramePath))
            {
                if (Path.GetExtension(path).Contains("meta"))
                    continue;
                if (Path.GetFileName(path).Contains("XFrame"))
                {
                    string toPath = Path.Combine(projectPath, Path.GetFileName(path));
                    File.Copy(path, toPath, true);
                    EditorLog.Debug($"{path} {EditorLog.Color("->", Color.green)} {toPath}");
                }
            }

            string guiSkinDirPath = Path.Combine(projectPath, "Resource");
            if (!Directory.Exists(guiSkinDirPath))
                Directory.CreateDirectory(guiSkinDirPath);

            string[] guiSkinPathesFrom = new string[]
            {
                Path.Combine(Application.dataPath, "UnityXFrame/Runtime/Debugger/DebugSkin.guiskin"),
                Path.Combine(Application.dataPath, "UnityXFrame/Runtime/Debugger/Res/pro.png"),
                Path.Combine(Application.dataPath, "UnityXFrame/Runtime/Audio/XAudioMixer.mixer")
            };

            foreach (string guiSkinPathFrom in guiSkinPathesFrom)
            {
                string guiSkinPath = Path.Combine(guiSkinDirPath, Path.GetFileName(guiSkinPathFrom));
                File.Copy(guiSkinPathFrom, guiSkinPath, true);
                EditorLog.Debug($"{guiSkinPathFrom} {EditorLog.Color("->", Color.green)} {guiSkinPath}");
            }
        }

        public void CopyToProject()
        {
            if (EditorApplication.isCompiling)
            {
                EditorLog.Debug($"Script is compiling");
                return;
            }
            foreach (string path in m_Data.ToProjectDllPath)
            {
                CopyToProject(path);
            }
        }
    }
}

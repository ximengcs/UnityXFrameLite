using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Player;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace UnityXFrame.Editor
{
    public class UsefulToolEditor : EditorWindow
    {
        private UsefulToolData m_Data;
        private GUIStyle m_Style1 => GUI.skin.customStyles[205];
        private GUIStyle m_Style2 => GUI.skin.customStyles[487];
        private GUIStyle m_Style3 => GUI.skin.customStyles[506];

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
            GUILayout.BeginVertical(m_Style1);
            EditorGUILayout.LabelField("XFrame", m_Style2);
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
            GUILayout.EndVertical();

            GUILayout.BeginVertical(m_Style1);
            EditorGUILayout.LabelField("UnityXFrame", m_Style2);
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
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy To", GUILayout.Width(80)))
                CopyToProject();
            m_Data.ToProjectDllPath = EditorGUILayout.TextField(m_Data.ToProjectDllPath);
            if (GUILayout.Button("→", GUILayout.Width(20)))
                EditorUtility.RevealInFinder(m_Data.ToProjectDllPath);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Compile & Copy"))
            {
                CompileDll();
                CopyToProject();
            }
            GUILayout.EndVertical();
            EditorUtility.SetDirty(m_Data);
        }

        public void CompileXFrame()
        {
            EditorLog.Debug("========================== START COMPILE XFRAME ========================");
            string fullPath = Path.Combine(m_Data.XFrameProjectPath, "XFrame.sln");
            ProcessStartInfo startInfo = new ProcessStartInfo("dotnet", "build " + fullPath);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = Process.Start(startInfo);
            process.OutputDataReceived += (sender, a) => EditorLog.Debug(a.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            EditorLog.Debug("=======================================================================");
        }

        public string XFrameDllPath()
        {
            string path = m_Data.XFrameProjectPath + "/XFrame/bin/{0}/netstandard2.1";
            path = string.Format(path, m_Data.IsRelease ? "Release" : "Debug");
            return path;
        }

        public void ImportXFrame()
        {
            string path = XFrameDllPath();
            foreach (string p in Directory.EnumerateFiles(path))
            {
                if (Path.GetFileName(p).Contains("XFrame"))
                {
                    string toPath = Path.Combine(m_Data.XFramePath, Path.GetFileName(p));
                    byte[] data = File.ReadAllBytes(p);
                    File.WriteAllBytes(toPath, data);
                    EditorLog.Debug($"{p}\t -> {toPath}");
                }
            }

            AssetDatabase.Refresh();
        }

        public void CompileDll()
        {
            if (Directory.Exists(m_Data.XFrameProjectPath))
            {
                foreach (string file in Directory.EnumerateFiles(m_Data.XFrameProjectPath))
                    File.Delete(file);
            }
            else
            {
                Directory.CreateDirectory(m_Data.XFrameProjectPath);
            }

            var group = BuildPipeline.GetBuildTargetGroup(m_Data.CurrentBuildTarget);
            ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
            scriptCompilationSettings.group = group;
            scriptCompilationSettings.target = m_Data.CurrentBuildTarget;
            Directory.CreateDirectory(m_Data.BuildDllPath);
            PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, m_Data.BuildDllPath);
        }

        public void CopyToProject()
        {
            if (!string.IsNullOrEmpty(m_Data.BuildDllPath))
            {
                foreach (string path in Directory.EnumerateFiles(m_Data.BuildDllPath))
                {
                    if (Path.GetFileName(path).Contains("UnityXFrame"))
                    {
                        string toPath = Path.Combine(m_Data.ToProjectDllPath, Path.GetFileName(path));
                        File.Copy(path, toPath, true);
                        EditorLog.Debug($"{path} -> {toPath}");
                    }
                }
            }

            string assemblyPath = Application.dataPath + "/../Library/ScriptAssemblies/";
            foreach (string path in Directory.EnumerateFiles(assemblyPath))
            {
                if (Path.GetFileName(path).Contains("UnityXFrame.Editor"))
                {
                    string toPath = Path.Combine(m_Data.ToProjectDllPath, "Editor", Path.GetFileName(path));
                    File.Copy(path, toPath, true);
                    EditorLog.Debug($"{path} -> {toPath}");
                }
            }

            foreach (string path in Directory.EnumerateFiles(m_Data.XFramePath))
            {
                if (Path.GetExtension(path).Contains("meta"))
                    continue;
                if (Path.GetFileName(path).Contains("XFrame"))
                {
                    string toPath = Path.Combine(m_Data.ToProjectDllPath, Path.GetFileName(path));
                    File.Copy(path, toPath, true);
                    EditorLog.Debug($"{path} -> {toPath}");
                }
            }
        }
    }
}

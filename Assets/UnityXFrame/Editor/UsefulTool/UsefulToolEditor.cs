using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Build.Player;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class UsefulToolEditor : EditorWindow
    {
        private string m_BuildPath;

        private void OnEnable()
        {

            titleContent = new GUIContent("Tool");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Clear Remote Build Path"))
            {
                var setting = AddressableAssetSettingsDefaultObject.Settings;
                string path = setting.RemoteCatalogBuildPath.GetValue(setting);
                foreach (string file in Directory.EnumerateFiles(path))
                {
                    string ext = Path.GetExtension(file);
                    if (ext == ".hash" || ext == ".json")
                        continue;

                    File.Delete(file);
                }
            }

            if (GUILayout.Button("Compile Editor Dll"))
            {

            }

            m_BuildPath = EditorGUILayout.TextField(m_BuildPath);
            if (GUILayout.Button("Compile Android Dll"))
            {
                CompileDll(m_BuildPath, BuildTarget.Android);
            }
        }

        public void CompileDll(string buildDir, BuildTarget target)
        {
            var group = BuildPipeline.GetBuildTargetGroup(target);
            ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
            scriptCompilationSettings.group = group;
            scriptCompilationSettings.target = target;
            Directory.CreateDirectory(buildDir);
            PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, buildDir);
        }
    }
}

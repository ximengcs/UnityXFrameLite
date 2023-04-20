using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class UsefulToolEditor : EditorWindow
    {
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
        }
    }
}

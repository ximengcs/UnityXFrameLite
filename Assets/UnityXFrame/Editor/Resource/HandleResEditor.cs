using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine;
using System.IO;
using UnityXFrame.Core;
using XFrame.Modules.Reflection;
using XFrame.Core;
using XFrame.Modules.Config;
using System;

namespace UnityXFrame.Editor
{
    public partial class HandleResEditor : EditorWindow
    {
        private string m_SavePath;
        private HandleResData m_Data;
        private SerializedObject m_Obj;
        private XCore m_FrameCore;
        private IAtlasResHandler m_Handler;

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

            m_SavePath = Constant.CONFIG_PATH;
            m_Obj = new SerializedObject(m_Data);
            TypeChecker.IncludeModule("Assembly-CSharp");
            TypeChecker.IncludeModule("Assembly-CSharp-Editor");
            TypeChecker.IncludeModule("UnityXFrame");
            TypeChecker.IncludeModule("UnityXFrame.Lib");
            TypeChecker.IncludeModule("UnityXFrame.Editor");
            XConfig.TypeChecker = new TypeChecker();
            m_FrameCore = XCore.Create(typeof(TypeModule));

            ITypeModule typeModule = m_FrameCore.GetModule<ITypeModule>();
            TypeSystem typeSystem = typeModule.GetOrNew<IAtlasResHandler>();

            foreach (Type type in typeSystem)
            {
                m_Handler = (IAtlasResHandler)typeModule.CreateInstance(type);
                break;
            }

            InnerInitAlias();
            InnerInitAtlas();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical(StyleUtility.Style1);
            InnerRenderAtlasGUI();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(StyleUtility.Style1);
            InnerRenderAliasGUI();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(StyleUtility.Style1);
            if (GUILayout.Button("Save Data"))
                m_Obj.ApplyModifiedProperties();
            GUILayout.EndVertical();
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
                if (string.IsNullOrEmpty(path))
                    path = Path.GetFileName(entry.AssetPath);
            }
            else
                path = null;
            return entry != null;
        }
    }
}

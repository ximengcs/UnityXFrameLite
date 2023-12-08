using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class Define
    {
        [MenuItem("UnityXFrame/Build AssetsBundle")]
        public static void BuildAB()
        {
            EditorWindow.GetWindow<AssetsEditor>().Show();
            //EditorWindow.GetWindow<BuildEditor>().Show();
        }

        [MenuItem("UnityXFrame/To Persist Folder")]
        public static void ToPersistFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("UnityXFrame/To Cache Folder")]
        public static void ToCacheFolder()
        {
            List<string> pathes = new List<string>();
            Caching.GetAllCachePaths(pathes);
            EditorUtility.RevealInFinder(pathes[0]);
        }

        [MenuItem("UnityXFrame/Useful Tool")]
        public static void UseFul()
        {
            EditorWindow.GetWindow<UsefulToolEditor>().Show();
        }

        [MenuItem("UnityXFrame/Test GUI Skin")]
        public static void TestGUISkin()
        {
            EditorWindow.GetWindow<TestGUISkinWindow>().Show();
        }

        [MenuItem("UnityXFrame/Clear User Data")]
        public static void ClearUserData()
        {
            PlayerPrefs.DeleteAll();
            if (Directory.Exists(Application.persistentDataPath))
                Directory.Delete(Application.persistentDataPath, true);
        }

        [MenuItem("UnityXFrame/Clear Cache Data")]
        public static void ClearCacheData()
        {
            Caching.ClearCache();
        }

        [MenuItem("UnityXFrame/Check Error")]
        public static void CheckError()
        {
            EditorWindow.GetWindow<CheckErrorEditor>().Show();
        }

        [MenuItem("UnityXFrame/Editor Icons %e")]
        public static void EditorIconsOpen()
        {
            EditorIcons w = EditorWindow.GetWindow<EditorIcons>("Editor Icons");
            w.ShowUtility();
            w.minSize = new Vector2(320, 450);
        }

        [MenuItem("UnityXFrame/Name Edit")]
        public static void Name()
        {
            NameEditor w = EditorWindow.GetWindow<NameEditor>("NameEditor");
            w.ShowUtility();
        }

        [MenuItem("UnityXFrame/Atlas Res")]
        public static void AtlasRes()
        {
            HandleResEditor w = EditorWindow.GetWindow<HandleResEditor>("AtlasResEditor");
            w.ShowUtility();
        }
    }
}
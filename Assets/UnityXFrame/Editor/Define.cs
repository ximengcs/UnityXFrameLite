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
            //EditorWindow.GetWindow<AssetsEditor>().Show();
            //EditorWindow.GetWindow<BuildEditor>().Show();
        }

        [MenuItem("UnityXFrame/To Persist Folder")]
        public static void ToPersistFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
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
    }
}
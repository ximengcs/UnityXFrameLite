using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class Define
    {
        [MenuItem("Tools/Build AssetsBundle")]
        public static void BuildAB()
        {
            EditorWindow.GetWindow<BuildEditor>().Show();
        }

        [MenuItem("Tools/To Persist Folder")]
        public static void ToPersistFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Tools/Useful Tool")]
        public static void UseFul()
        {
            EditorWindow.GetWindow<UsefulToolEditor>().Show();
        }

        [MenuItem("Tools/Test GUI Skin")]
        public static void TestGUISkin()
        {
            EditorWindow.GetWindow<TestGUISkinWindow>().Show();
        }
    }
}
using UnityEditor;
using UnityEngine;
using System.Text;
using UnityEditor.Build;
using System.Collections.Generic;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class Utility
        {
            private static HashSet<string> m_Symbols;

            public static void Init()
            {
                m_Symbols = new HashSet<string>();
                string symbol = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone);
                m_Symbols = new HashSet<string>(symbol.Split(';'));
            }

            public static bool ContainsSymbol(string name)
            {
                return m_Symbols.Contains(name);
            }

            public static void AddSymbol(string name)
            {
                m_Symbols.Add(name);
                InnerSaveSymbol();
            }

            public static void RemoveSymbol(string name)
            {
                m_Symbols.Remove(name);
                InnerSaveSymbol();
            }

            private static void InnerSaveSymbol()
            {
                StringBuilder symbols = new StringBuilder();
                foreach (string symbol in m_Symbols)
                {
                    symbols.Append(symbol);
                    symbols.Append(';');
                }

                PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, symbols.ToString());
                PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Android, symbols.ToString());
                PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.iOS, symbols.ToString());
            }

            public static void Lable(string lable)
            {
                EditorGUILayout.LabelField(lable, GUI.skin.customStyles[40], GUILayout.Width(100));
            }

            public static bool Toggle(string lable, bool vale)
            {
                return EditorGUILayout.Toggle(lable, vale, GUI.skin.customStyles[148]);
            }

            public static bool Toggle(bool vale)
            {
                return EditorGUILayout.Toggle(vale, GUI.skin.customStyles[148]);
            }
        }
    }
}

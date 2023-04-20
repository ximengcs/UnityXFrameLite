
using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class TestGUISkinWindow : EditorWindow
    {
        private Vector2 m_Pos;
        private string m_Icon;
        private Texture2D m_Tex;

        private void OnGUI()
        {
            if (!string.IsNullOrEmpty(m_Icon))
            {
                GUIContent content = EditorGUIUtility.IconContent(m_Icon);
                m_Tex = content.image as Texture2D;
            }
            m_Tex = (Texture2D)EditorGUILayout.ObjectField(m_Tex, typeof(Texture2D), false);

            m_Icon = GUILayout.TextField(m_Icon);
            m_Pos = GUILayout.BeginScrollView(m_Pos);
            GUIStyle[] skins = GUI.skin.customStyles;
            for (int i = 0; i < skins.Length; i++)
            {
                GUILayout.Label($"{i}", skins[i]);
            }
            GUILayout.EndScrollView();
        }
    }
}

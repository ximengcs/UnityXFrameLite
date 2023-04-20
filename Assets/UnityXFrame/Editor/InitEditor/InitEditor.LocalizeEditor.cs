using UnityEditor;
using UnityEngine;
using XFrame.Modules.Local;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class LocalizeEditor : DataEditorBase
        {
            private TextAsset m_File;

            public override void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Language");
                Language lang = m_Data.Language;
                m_Data.Language = (Language)EditorGUILayout.EnumPopup(m_Data.Language);
                if (m_Data.Language != lang)
                    EditorUtility.SetDirty(m_Data);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Utility.Lable("LocalizeFile");
                m_File = (TextAsset)EditorGUILayout.ObjectField(m_Data.LocalizeFile, typeof(TextAsset), false);
                EditorGUILayout.EndHorizontal();

                if (m_File != m_Data.LocalizeFile)
                {
                    m_Data.LocalizeFile = m_File;
                    EditorUtility.SetDirty(m_Data);
                }
            }
        }
    }
}

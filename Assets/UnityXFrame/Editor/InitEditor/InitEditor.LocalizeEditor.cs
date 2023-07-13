using UnityEditor;
using UnityEngine;
using XFrame.Modules.Local;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class LocalizeEditor : DataEditorBase
        {
            public override void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Language");
                Language lang = m_Data.Language;
                m_Data.Language = (Language)EditorGUILayout.EnumPopup(m_Data.Language);
                if (m_Data.Language != lang)
                    EditorUtility.SetDirty(m_Data);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}

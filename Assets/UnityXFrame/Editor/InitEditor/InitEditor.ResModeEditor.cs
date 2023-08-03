using System;
using UnityEditor;
using XFrame.Modules.XType;
using XFrame.Modules.Resource;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class ResModeEditor : DataEditorBase
        {
            private TypeSystem m_ResHelperTypes;
            private Type[] m_Types;
            private string[] m_ResHelperTypeNames;
            private int m_ResHelperTypeIndex;

            protected override void OnInit()
            {
                m_ResHelperTypes = m_Inst.m_FrameCore.GetModule<TypeModule>().GetOrNew<IResourceHelper>();

                m_Types = m_ResHelperTypes.ToArray();
                m_ResHelperTypeNames = new string[m_Types.Length];
                for (int i = 0; i < m_Types.Length; i++)
                {
                    m_ResHelperTypeNames[i] = m_Types[i].Name;
                    if (m_Types[i].FullName == m_Data.ResMode)
                        m_ResHelperTypeIndex = i;
                }

                if (string.IsNullOrEmpty(m_Data.ResMode) && m_ResHelperTypeNames.Length > 0)
                    InnerSelect(0);
            }

            public override void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Res Mode");
                int index = m_ResHelperTypeIndex;
                m_ResHelperTypeIndex = EditorGUILayout.Popup(m_ResHelperTypeIndex, m_ResHelperTypeNames);
                EditorGUILayout.EndHorizontal();

                if (index != m_ResHelperTypeIndex)
                    InnerSelect(m_ResHelperTypeIndex);
            }

            private void InnerSelect(int index)
            {
                m_ResHelperTypeIndex = index;
                m_Data.ResMode = m_Types[m_ResHelperTypeIndex].FullName;
                EditorUtility.SetDirty(m_Data);
            }
        }
    }
}

using System;
using UnityEditor;
using XFrame.Modules.Download;
using XFrame.Modules.XType;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class NetworkEditor : DataEditorBase
        {
            private TypeSystem m_HelperTypes;
            private Type[] Types;
            private string[] m_HelperTypeNames;
            private int m_HelperTypeIndex;

            protected override void OnInit()
            {
                m_HelperTypes = TypeModule.Inst.GetOrNew<IDownloadHelper>();

                Types = m_HelperTypes.ToArray();
                m_HelperTypeNames = new string[Types.Length];
                for (int i = 0; i < Types.Length; i++)
                {
                    string name = Types[i].Name;
                    m_HelperTypeNames[i] = name;
                    if (name == m_Data.DownloadHelper)
                        m_HelperTypeIndex = i;
                }

                if (string.IsNullOrEmpty(m_Data.DownloadHelper) && m_HelperTypeNames.Length > 0)
                    InnerSelect(0);
            }

            public override void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("DownloadHelper");
                int index = m_HelperTypeIndex;
                m_HelperTypeIndex = EditorGUILayout.Popup(m_HelperTypeIndex, m_HelperTypeNames);
                if (index != m_HelperTypeIndex)
                    InnerSelect(m_HelperTypeIndex);
                EditorGUILayout.EndHorizontal();
            }

            private void InnerSelect(int index)
            {
                m_HelperTypeIndex = index;
                m_Data.DownloadHelper = Types[m_HelperTypeIndex].FullName;
                EditorUtility.SetDirty(m_Data);
            }
        }
    }
}

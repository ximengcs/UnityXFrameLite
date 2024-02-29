using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Archives;
using XFrame.Modules.Download;
using XFrame.Modules.Reflection;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class PathEditor : DataEditorBase
        {
            private TypeSystem m_HelperTypes;
            private Type[] Types;
            private string[] m_HelperTypeNames;
            private int m_HelperTypeIndex;

            protected override void OnInit()
            {
                m_Data.ArchivePath = Constant.ArchivePath;
                EditorUtility.SetDirty(m_Data);

                m_HelperTypes = m_Inst.m_FrameCore.GetModule<TypeModule>().GetOrNew<IArchiveUtilityHelper>();

                List<Type> list = new List<Type>();
                list.Add(null);
                foreach (Type type in m_HelperTypes)
                {
                    if (type.Name.StartsWith("Default"))
                        list[0] = type;
                    else
                        list.Add(type);
                }
                Types = list.ToArray();
                m_HelperTypeNames = new string[Types.Length];
                for (int i = 0; i < Types.Length; i++)
                {
                    string name = Types[i].FullName;
                    m_HelperTypeNames[i] = Types[i].Name;
                    if (name == m_Data.ArchiveUtilityHelper)
                        m_HelperTypeIndex = i;
                }

                if (string.IsNullOrEmpty(m_Data.ArchiveUtilityHelper) && m_HelperTypeNames.Length > 0)
                    InnerSelect(0);
            }

            public override void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("ArchivePath");
                string path = m_Data.ArchivePath;
                int length = 20;
                if (path.Length > length)
                    path = path.Substring(0, length) + "...";
                if (GUILayout.Button(path))
                {
                    if (!Directory.Exists(m_Data.ArchivePath))
                        Directory.CreateDirectory(m_Data.ArchivePath);
                    EditorUtility.RevealInFinder(m_Data.ArchivePath);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Utility.Lable("ArchiveUtilityHelper");
                int index = m_HelperTypeIndex;
                m_HelperTypeIndex = EditorGUILayout.Popup(m_HelperTypeIndex, m_HelperTypeNames);
                if (index != m_HelperTypeIndex)
                    InnerSelect(m_HelperTypeIndex);
                EditorGUILayout.EndHorizontal();
            }

            private void InnerSelect(int index)
            {
                m_HelperTypeIndex = index;
                m_Data.ArchiveUtilityHelper = Types[m_HelperTypeIndex].FullName;
                EditorUtility.SetDirty(m_Data);
            }
        }
    }
}

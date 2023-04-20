using System.IO;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class PathEditor : DataEditorBase
        {
            protected override void OnInit()
            {
                m_Data.ArchivePath = Constant.ArchivePath;
                EditorUtility.SetDirty(m_Data);
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
            }
        }
    }
}

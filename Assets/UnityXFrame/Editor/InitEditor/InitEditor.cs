using System;
using XFrame.Core;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Config;
using UnityEditor.SceneManagement;
using XFrame.Modules.Reflection;

namespace UnityXFrame.Editor
{
    [CustomEditor(typeof(Init))]
    public partial class InitEditor : UnityEditor.Editor
    {
        public const string InitDataPath = "Assets/UnityXFrame/InitData.asset";

        private static InitEditor m_Inst;
        private XCore m_FrameCore;
        private InitData m_Data;
        private TypeSystem m_EditorType;
        private XLinkList<IDataEditor> m_Editors;

        private void OnEnable()
        {
            if (Application.isPlaying)
                return;
            m_Inst = this;
            Utility.Init();
            XConfig.UseClassModule = new string[] { "Assembly-CSharp", "UnityXFrame", "UnityXFrame.Lib", "UnityXFrame.Editor" };
            m_FrameCore = XCore.Create(typeof(TypeModule));
            m_Editors = new XLinkList<IDataEditor>();
            m_EditorType = m_FrameCore.GetModule<TypeModule>().GetOrNew<IDataEditor>();
            m_Data = AssetDatabase.LoadAssetAtPath<InitData>(InitDataPath);
            if (m_Data == null)
            {
                m_Data = CreateInstance<InitData>();
                AssetDatabase.CreateAsset(m_Data, InitDataPath);
            }

            SerializedProperty dataProp = serializedObject.FindProperty("Data");
            if (dataProp.objectReferenceValue != m_Data)
            {
                dataProp.objectReferenceValue = m_Data;
                serializedObject.ApplyModifiedProperties();
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }

            foreach (Type type in m_EditorType)
            {
                if (type.IsAbstract)
                    continue;
                IDataEditor editor = Activator.CreateInstance(type) as IDataEditor;
                editor.OnInit(m_Data);
                m_Editors.AddLast(editor);
            }
        }

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
                return;
            XLinkNode<IDataEditor> node = m_Editors.First;
            while (node != null)
            {
                IDataEditor editor = node.Value;
                editor.Enable = Utility.Toggle(editor.Name, editor.Enable);
                if (editor.Enable)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.customStyles[40]);
                    editor.OnUpdate();
                    EditorGUILayout.EndVertical();
                }

                node = node.Next;
            }
        }

        private void OnDisable()
        {
            if (Application.isPlaying)
                return;
            if (m_Editors == null)
                return;

            XLinkNode<IDataEditor> node = m_Editors.First;
            while (node != null)
            {
                node.Value.OnDestroy();
                node = node.Next;
            }

            EditorUtility.SetDirty(m_Data);
            m_FrameCore.Destroy();
            m_EditorType = null;
            m_Editors = null;
            m_Data = null;
        }
    }
}

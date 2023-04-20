using UnityEditor;
using UnityEngine.Audio;
using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class AudioEditor : DataEditorBase
        {
            private AudioMixer m_File;

            public override void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("AudioMixer");
                m_File = (AudioMixer)EditorGUILayout.ObjectField(m_Data.AudioMixer, typeof(AudioMixer), false);
                EditorGUILayout.EndHorizontal();

                if (m_File != m_Data.AudioMixer)
                {
                    m_Data.AudioMixer = m_File;
                    EditorUtility.SetDirty(m_Data);
                }
            }
        }
    }
}

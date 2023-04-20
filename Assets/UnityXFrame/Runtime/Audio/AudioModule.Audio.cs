using System;
using UnityEngine;
using UnityEngine.Audio;
using XFrame.Modules.Pools;
using XFrame.Modules.Tasks;

namespace UnityXFrame.Core.Audios
{
    public partial class AudioModule
    {
        private class Audio : IAudio
        {
            private GameObject m_Inst;
            private AudioSource m_Source;
            private float m_Volume;

            private float m_DefaultVolume;
            private BolActionTask m_WaitTask;
            private Group m_Group;

            private Action m_OnDispose;

            public AudioClip Clip
            {
                get => m_Source.clip;
                set
                {
                    m_Source.clip = value;
                    if (value != null)
                        m_Inst.name = value.name;
                }
            }

            public float Volume
            {
                get => m_Volume;
                set
                {
                    m_Volume = value;
                    InnerUpdateSourceVolume();
                }
            }

            public IAudioGroup Group => m_Group;

            public void OnInit(Transform root, AudioMixerGroup group, AudioClip clip)
            {
                m_Volume = 1.0f;
                m_Inst = new GameObject(clip.name);
                m_Inst.transform.SetParent(root);
                m_Source = m_Inst.AddComponent<AudioSource>();
                m_Source.outputAudioMixerGroup = group;
                m_DefaultVolume = m_Source.volume;
                Clip = clip;
            }

            public void SetGroup(Group group)
            {
                m_Group = group;
            }

            public void Play(Action callback = null)
            {
                m_Source.loop = false;
                InnerUpdateSourceVolume();
                m_Source.Play();

                if (m_WaitTask != null)
                {
                    m_WaitTask.Delete();
                    m_WaitTask = null;
                }

                m_WaitTask = TaskModule.Inst.GetOrNew<BolActionTask>();
                m_WaitTask.Add(() => !m_Source.isPlaying)
                    .OnComplete(() =>
                    {
                        m_WaitTask = null;
                        callback?.Invoke();
                        m_OnDispose?.Invoke();
                    })
                    .Start();
            }

            public void PlayLoop()
            {
                m_Source.loop = true;
                InnerUpdateSourceVolume();
                m_Source.Play();
            }

            public void Stop()
            {
                m_Source.Stop();
                m_OnDispose?.Invoke();
            }

            public void OnDispose(Action callback)
            {
                m_OnDispose += callback;
            }

            void IPoolObject.OnCreate()
            {
                m_Inst?.SetActive(true);
                if (m_Source != null)
                    m_Source.volume = m_DefaultVolume;
            }

            void IPoolObject.OnDelete()
            {
                GameObject.Destroy(m_Inst);
                m_Inst = null;
                m_Source = null;
            }

            void IPoolObject.OnRelease()
            {
                Clip = null;
                if (m_WaitTask != null)
                {
                    m_WaitTask.Delete();
                    m_WaitTask = null;
                }

                m_Group = null;
                m_Inst.SetActive(false);
                m_OnDispose = null;
            }

            private void InnerUpdateSourceVolume()
            {
                m_Source.volume = Inst.Volume * Group.Volume * m_Volume;
            }
        }
    }
}

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

            private BolActionTask m_WaitTask;
            private Group m_Group;

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
                m_Inst.name = clip.name;
                m_Inst.transform.SetParent(root);
                m_Source.outputAudioMixerGroup = group;
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
                        Stop();
                    }).Start();
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
                PoolModule.Inst.GetOrNew<Audio>().Release(this);
                m_Group.Remove(this);
            }

            int IPoolObject.PoolKey => 0;

            void IPoolObject.OnCreate()
            {
                m_Inst = new GameObject("Audio");
                m_Source = m_Inst.AddComponent<AudioSource>();
            }

            void IPoolObject.OnRequest()
            {
                m_Inst.SetActive(true);
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
            }

            private void InnerUpdateSourceVolume()
            {
                m_Source.volume = Inst.Volume * Group.Volume * m_Volume;
            }
        }
    }
}

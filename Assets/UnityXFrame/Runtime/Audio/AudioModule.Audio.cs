using DG.Tweening;
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
            private bool m_Disposed;
            private bool m_Paused;

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

            public void Pause()
            {
                if (m_Disposed)
                    return;
                if (!m_Paused)
                {
                    m_Paused = true;
                    m_Source.Pause();
                }
            }

            public void Play(Action callback = null)
            {
                if (m_Disposed)
                    return;
                if (m_Paused)
                {
                    m_Paused = false;
                    m_Source.Play();
                    return;
                }

                m_Source.loop = false;
                InnerUpdateSourceVolume();
                m_Source.Play();
                if (m_WaitTask != null)
                {
                    m_WaitTask.Delete();
                    m_WaitTask = null;
                }

                m_WaitTask = TaskModule.Inst.GetOrNew<BolActionTask>();
                m_WaitTask.Add(() => !m_Source.isPlaying && !m_Paused)
                    .OnComplete(() =>
                    {
                        m_WaitTask = null;
                        callback?.Invoke();
                        Stop();
                    }).Start();
            }

            public void PlayLoop()
            {
                if (m_Disposed)
                    return;
                m_Paused = false;
                m_Source.loop = true;
                InnerUpdateSourceVolume();
                m_Source.Play();
            }

            public void Stop()
            {
                if (m_Disposed)
                    return;
                m_Source.Stop();
                m_Group.Remove(this);
                PoolModule.Inst.GetOrNew<Audio>().Release(this);
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
                m_Disposed = false;
                m_Paused = false;
            }

            void IPoolObject.OnDelete()
            {
                GameObject.Destroy(m_Inst);
                m_Inst = null;
                m_Source = null;
            }

            void IPoolObject.OnRelease()
            {
                m_Disposed = true;
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

using System;
using UnityEngine;
using UnityEngine.Audio;
using XFrame.Modules.Pools;
using XFrame.Modules.Tasks;
using XFrame.Modules.Diagnotics;
using XFrame.Core;
using XFrame.Tasks;

namespace UnityXFrame.Core.Audios
{
    public partial class AudioModule
    {
        private class Audio : IAudio
        {
            private string m_Name;
            private GameObject m_Inst;
            private AudioSource m_Source;
            private float m_Volume;
            private bool m_Disposed;
            private bool m_Paused;
            private bool m_AutoRelease;
            private Action m_PlayCallback;

            private ITask m_WaitTask;
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

            public bool AutoRelease
            {
                get => m_AutoRelease;
                set
                {
                    if (m_Disposed)
                    {
                        Log.Debug("Audio", $"{m_Name} has release, but you try set AutoRelease");
                        return;
                    }

                    if (value != m_AutoRelease)
                    {
                        m_AutoRelease = value;
                        if (m_WaitTask == null)
                        {
                            m_WaitTask = XTask.Condition(() =>
                            {
                                return !m_Source.isPlaying && !m_Paused;
                            })
                            .OnCompleted(() =>
                            {
                                m_WaitTask = null;
                                m_PlayCallback?.Invoke();
                                m_PlayCallback = null;
                                if (m_Disposed)
                                    return;
                                Release();
                            });
                            m_WaitTask.Coroutine();
                        }
                    }
                }
            }

            public bool IsDisposed => m_Disposed;

            public float Volume
            {
                get => m_Volume;
                set
                {
                    if (m_Disposed)
                    {
                        Log.Debug("Audio", $"{m_Name} has release, but you try set Volume");
                        return;
                    }
                    m_Volume = value;
                    InnerUpdateSourceVolume();
                }
            }

            public IAudioGroup Group => m_Group;

            public void OnInit(Transform root, AudioMixerGroup group, AudioClip clip, bool autoRelease)
            {
                m_Volume = 1.0f;
                m_Name = clip.name;
                m_Inst.name = m_Name;
                m_AutoRelease = autoRelease;
                m_Inst.transform.SetParent(root);
                m_Source.outputAudioMixerGroup = group;
                Clip = clip;
            }

            public void SetGroup(Group group)
            {
                if (m_Disposed)
                {
                    Log.Debug("Audio", $"{m_Name} has release, but you try SetGroup");
                    return;
                }
                m_Group = group;
            }

            public void Pause()
            {
                if (m_Disposed)
                {
                    Log.Debug("Audio", $"{m_Name} has release, but you try Pause");
                    return;
                }
                if (!m_Paused)
                {
                    m_Paused = true;
                    m_Source.Pause();
                }
            }

            public void Continue()
            {
                if (m_Disposed)
                {
                    Log.Debug("Audio", $"{m_Name} has release, but you try Continue");
                    return;
                }
                if (m_Paused)
                {
                    m_Paused = false;
                    m_Source.Play();
                }
            }

            public void Release()
            {
                if (m_Disposed)
                {
                    Log.Debug("Audio", $"{m_Name} has release, but you try Release again");
                    return;
                }

                m_Group.Remove(this);
                Global.Pool.GetOrNew<Audio>().Release(this);
            }

            public void Play(Action callback = null)
            {
                if (m_Disposed)
                {
                    Log.Debug("Audio", $"{m_Name} has release, but you try Play");
                    return;
                }

                m_PlayCallback = callback;
                m_Paused = false;
                m_Source.loop = false;
                m_Source.time = 0;
                InnerUpdateSourceVolume();
                m_Source.Play();
                if (m_WaitTask != null)
                {
                    m_WaitTask.Cancel(true);
                    m_WaitTask = null;
                }

                if (AutoRelease)
                {
                    m_WaitTask = XTask.Condition(() => !m_Source.isPlaying && !m_Paused)
                        .OnCompleted(() =>
                        {
                            m_WaitTask = null;
                            m_PlayCallback?.Invoke();
                            m_PlayCallback = null;
                            if (m_Disposed)
                                return;
                            Release();
                        });
                    m_WaitTask.Coroutine();
                }
            }

            public void PlayLoop()
            {
                if (m_Disposed)
                {
                    Log.Debug("Audio", $"{m_Name} has release, but you try PlayLoop");
                    return;
                }

                m_Source.time = 0;
                m_Paused = false;
                m_Source.loop = true;
                InnerUpdateSourceVolume();
                m_Source.Play();
            }

            public void Stop()
            {
                if (m_Disposed)
                {
                    Log.Debug("Audio", $"{m_Name} has release, but you try Stop");
                    return;
                }
                m_Source.Stop();
                if (AutoRelease)
                    Release();
            }

            int IPoolObject.PoolKey => 0;

            public string MarkName { get; set; }

            IPool IPoolObject.InPool { get; set; }

            void IPoolObject.OnCreate(IPoolModule module)
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
                    m_WaitTask.Cancel(true);
                    m_WaitTask = null;
                }

                m_Group = null;
                m_Inst.SetActive(false);
            }

            private void InnerUpdateSourceVolume()
            {
                m_Source.volume = Global.Audio.Volume * Group.Volume * m_Volume;
            }
        }
    }
}

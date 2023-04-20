using System.Collections.Generic;

namespace UnityXFrame.Core.Audios
{
    public partial class AudioModule
    {
        private class Group : IAudioGroup
        {
            private float m_Volume;
            private List<IAudio> m_Audios;

            public float Volume
            {
                get => m_Volume;
                set
                {
                    m_Volume = value;
                    foreach (IAudio audio in m_Audios)
                        audio.Volume = audio.Volume;
                }
            }

            public void OnInit()
            {
                m_Volume = 1.0f;
                m_Audios = new List<IAudio>();
            }

            public void OnDestroy()
            {
                foreach (IAudio audio in m_Audios)
                    audio.Stop();
                m_Audios.Clear();
            }

            public void Add(IAudio audio)
            {
                Audio realAudio = audio as Audio;
                Group group = realAudio.Group as Group;
                group?.Remove(realAudio);
                realAudio.SetGroup(this);
                m_Audios.Add(audio);
                realAudio.OnDispose(() => Remove(audio));
            }

            public void Remove(IAudio audio)
            {
                if (m_Audios.Contains(audio))
                    m_Audios.Remove(audio);
            }

            public void Play()
            {
                foreach (IAudio audio in m_Audios)
                    audio.Play();
            }

            public void Stop()
            {
                foreach (IAudio audio in m_Audios)
                    audio.Stop();
            }
        }
    }
}

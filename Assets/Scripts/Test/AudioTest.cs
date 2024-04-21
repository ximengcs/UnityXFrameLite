
using UnityXFrame.Core;
using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using XFrame.Core;

namespace Game.Test
{
    [DebugWindow]
    public class AudioTest : IDebugWindow
    {
        private IAudio m_Audio;
        private string m_Name;
        private bool m_Auto;

        public void OnAwake()
        {
            m_Name = "a1.wav";
        }

        public void OnDraw()
        {
            if (m_Audio != null)
                DebugGUI.Label($"Disposed -> {m_Audio.IsDisposed}");
            m_Auto = DebugGUI.Power(m_Auto);
            if (DebugGUI.Button("Play"))
            {
                Global.Audio.PlayAsync(m_Name, m_Auto)
                    .OnCompleted((audio) =>
                    {
                        m_Audio = audio;
                    });
            }
            if (DebugGUI.Button("PlayLoop"))
            {
                Global.Audio.PlayLoopAsync(m_Name, m_Auto)
                    .OnCompleted((audio) =>
                    {
                        m_Audio = audio;
                    });
            }
            if (DebugGUI.Button("Stop"))
            {
                m_Audio?.Stop();
            }
            if (DebugGUI.Button("Pause"))
            {
                m_Audio?.Pause();
            }
            if (DebugGUI.Button("Play"))
            {
                m_Audio?.Play();
            }
            if (DebugGUI.Button("PlayLoop"))
            {
                m_Audio?.PlayLoop();
            }
            if (DebugGUI.Button("Continue"))
            {
                m_Audio?.Continue();
            }
            if (DebugGUI.Button("Release"))
            {
                m_Audio?.Release();
            }
        }

        public void Dispose()
        {

        }
    }
}


using UnityXFrame.Core.Diagnotics;

namespace Game.Test
{
    [DebugWindow(1000)]
    public class ServerCase : IDebugWindow
    {
        private string m_Content;

        public void Dispose()
        {
            m_Content = string.Empty;
        }

        public void OnAwake()
        {
        }

        public void OnDraw()
        {

            m_Content = DebugGUI.TextField(m_Content);
            if (DebugGUI.Button("Send"))
            {
                World.Net.Connect.Send(m_Content);
            }
        }
    }
}

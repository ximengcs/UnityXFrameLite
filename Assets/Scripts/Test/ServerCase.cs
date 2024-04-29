
using UnityXFrame.Core.Diagnotics;

namespace Game.Test
{
    [DebugWindow(1000)]
    public class ServerCase : IDebugWindow
    {
        private string m_Content;
        private string m_IP;

        public void Dispose()
        {
        }

        public void OnAwake()
        {
            m_Content = string.Empty;
            m_IP = "192.168.137.1";
        }

        public void OnDraw()
        {
            m_IP = DebugGUI.TextField(m_IP);
            if (DebugGUI.Button("Connect"))
            {
                World.Net.ConnectServer(m_IP);
            }

            m_Content = DebugGUI.TextField(m_Content);
            if (DebugGUI.Button("Send"))
            {
                World.Net.Connect.Send(m_Content);
            }
        }
    }
}

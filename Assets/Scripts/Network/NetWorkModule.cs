
using System.Net.Sockets;
using System.Net;
using System;
using UnityEditor.MemoryProfiler;
using XFrame.Core;
using XFrame.Modules.Diagnotics;

namespace Game.Network
{
    [CommonModule]
    public partial class NetWorkModule : ModuleBase, IUpdater
    {
        private IPHostEntry m_IPHost;
        private IPAddress m_IPAddress;
        private IPEndPoint m_IPEndPoint;
        private Socket m_Sender;

        private Connection m_Connect;

        public Connection Connect => m_Connect;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_IPHost = Dns.GetHostEntry(Dns.GetHostName());
            m_IPAddress = m_IPHost.AddressList[0];
            m_IPEndPoint = new IPEndPoint(m_IPAddress, 9999);
            m_Sender = new Socket(m_IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        void IUpdater.OnUpdate(float escapeTime)
        {
            try
            {
                if (m_Connect == null)
                    m_Connect = new Connection(m_Sender, m_IPEndPoint);
                if (m_Connect != null)
                    m_Connect.Update();
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }
    }
}

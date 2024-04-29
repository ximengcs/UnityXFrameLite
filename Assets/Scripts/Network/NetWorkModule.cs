
using System.Net.Sockets;
using System.Net;
using System;
using XFrame.Core;
using XFrame.Modules.Diagnotics;

namespace Game.Network
{
    [CommonModule]
    public partial class NetWorkModule : ModuleBase, IUpdater
    {
        private IPEndPoint m_IPEndPoint;
        private Socket m_Sender;

        private Connection m_Connect;

        public Connection Connect => m_Connect;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
        }

        public void ConnectServer(string ip)
        {
            if (m_Connect != null)
                return;
            if (IPAddress.TryParse(ip, out IPAddress address))
            {
                m_IPEndPoint = new IPEndPoint(address, 9999);
                m_Sender = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    m_Connect = new Connection(m_Sender, m_IPEndPoint);
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }
        }

        void IUpdater.OnUpdate(float escapeTime)
        {
            if (m_Connect != null)
                m_Connect.Update();
        }
    }
}

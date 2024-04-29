
using System.Net;
using System.Net.Sockets;
using System.Text;
using XFrame.Modules.Diagnotics;

namespace Game.Network
{
    public partial class NetWorkModule
    {
        public class Connection
        {
            private Socket m_Sender;
            private IPEndPoint m_IP;

            private byte[] m_Cache;
            private byte[] m_BeatData;

            public bool Connecting { get; private set; }

            public Connection(Socket sender, IPEndPoint ip)
            {
                m_Sender = sender;
                m_IP = ip;
                m_Cache = new byte[1024];
                m_BeatData = new byte[8];
                Start();
            }

            public void Update()
            {
                if (Connecting)
                {
                    InnerReceiveData();
                }
            }

            public void Send(string content)
            {
                if (Connecting)
                {
                    m_Sender.Send(Encoding.UTF8.GetBytes(content));
                }
            }

            private void InnerReceiveData()
            {
                //if(m_Client.ReceiveAsync()
                //int byteCount = await m_Client.ReceiveAsync(m_Cache);
                //string content = Encoding.UTF8.GetString(m_Cache, 0, byteCount);
                //Log.Debug(content);
            }

            public void Start()
            {
                Log.Debug($"Start connect {m_Sender.Available}");
                m_Sender.Connect(m_IP);
                Connecting = true;
            }
        }
    }
}

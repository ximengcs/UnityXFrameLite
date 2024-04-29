
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using XFrame.Modules.Diagnotics;
using XFrame.Tasks;

namespace Game.Network
{
    public partial class NetWorkModule
    {
        public class Connection
        {
            private Socket m_Sender;
            private IPEndPoint m_IP;
            private XTask m_ConnectTask;
            private XTask m_ReceiveTask;

            private ArraySegment<byte> m_Cache;

            public bool Connecting { get; private set; }

            public Connection(Socket sender, IPEndPoint ip)
            {
                m_Sender = sender;
                m_IP = ip;
                m_Cache = new byte[1024];
                InnerConnect();
            }

            public void Update()
            {
                if (Connecting)
                {
                    if (m_ReceiveTask == null)
                    {
                        m_ReceiveTask = InnerReceiveData();
                        m_ReceiveTask.Coroutine();
                    }
                }
                else
                {
                    InnerConnect();
                }
            }

            private void InnerConnect()
            {
                if (m_ConnectTask != null)
                    return;
                m_ConnectTask = Start();
                m_ConnectTask.Coroutine();
            }

            public void Send(string content)
            {
                if (Connecting)
                {
                    Log.Debug($"send data");
                    m_Sender.Send(Encoding.UTF8.GetBytes(content.PadRight(5, ' ')));
                    Log.Debug($"send success");
                }
            }

            private async XTask InnerReceiveData()
            {
                int byteCount = await m_Sender.ReceiveAsync(m_Cache, SocketFlags.None);
                if (byteCount == 4)
                {
                    int code = BitConverter.ToInt32(m_Cache.Array, 0);
                    Log.Debug($"beat {code}");
                    await m_Sender.SendAsync(new ArraySegment<byte>(BitConverter.GetBytes(code)), SocketFlags.None);
                }
                else
                {
                    string content = Encoding.UTF8.GetString(m_Cache.Array, 0, byteCount);
                    Log.Debug(content);
                }
                m_ReceiveTask = null;
            }

            public async XTask Start()
            {
                Log.Debug($"Start connect");
                try
                {
                    await m_Sender.ConnectAsync(m_IP);
                    Log.Debug($"connect success");
                    Connecting = true;
                }
                catch
                {
                    Log.Error($"Connect failure");
                    Connecting = false;
                }
                m_ConnectTask = null;
            }
        }
    }
}

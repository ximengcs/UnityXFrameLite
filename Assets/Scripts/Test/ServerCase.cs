
using Assets.Scripts.Entities;
using Faker;
using Google.Protobuf;
using System;
using System.Net;
using UnityEngine;
using UnityXFrame.Core.Diagnotics;
using XFrame.Core;
using XFrame.Modules.Entities;
using XFrame.Tasks;
using XFrameShare.Network;

namespace Game.Test
{
    [DebugWindow(1000)]
    public class ServerCase : IDebugWindow
    {
        private string m_Content;
        private string m_IP;
        private XFrameServer.Test.Entities.Game m_Root;
        private int m_SendTimes;
        private int m_SuccessTimes;

        public void Dispose()
        {
        }

        public void OnAwake()
        {
            m_Content = string.Empty;
            m_IP = "192.168.0.104";
        }

        public void OnDraw()
        {
            m_IP = DebugGUI.TextField(m_IP);
            if (DebugGUI.Button("Connect"))
            {
                if (IPAddress.TryParse(m_IP, out IPAddress address))
                {
                    InnerConnect(address);
                }

            }

            m_Content = DebugGUI.TextField(m_Content);
            if (DebugGUI.Button("Send"))
            {
                InnerSend();
            }

            if (DebugGUI.Button("TestSend"))
            {
                TestSend();
            }

            if (DebugGUI.Button("TestReceive"))
            {
                TestReceive();
            }

            DebugGUI.Label(m_SendTimes.ToString());
            DebugGUI.Label(m_SuccessTimes.ToString());
            if (DebugGUI.Button("TestBeat"))
            {
                InnerTestBeat();
            }
        }

        private void InnerConnect(IPAddress address)
        {

        }

        private void InnerTestBeat()
        {
            MailBoxCom mail = m_Root.GetCom<MailBoxCom>();
            XTask.Beat(0.2f, () =>
            {
                m_SendTimes++;
                InnerSend();
                return false;
            }).Coroutine();
        }

        private void TestReceive()
        {
            //for (int i = 0; i < 10; i++)
            //    m_Root.GetCom<MailBoxCom>().m_Mail.ReceiveFactory().Coroutine();
        }

        private void TestSend()
        {
            for (int i = 0; i < 10; i++)
                InnerSend();
        }

        private async void InnerSend()
        {

        }
    }
}

using Assets.Scripts.Entities;
using System.Net;
using TestGame.Share.Clients;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityXFrame.Core;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.UIElements;
using UnityXFrameLib.UIElements;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrame.Modules.Times;
using XFrameShare.Network;

namespace Assets.Scripts.Test
{
    [AutoLoadUI]
    [AutoSpwanUI]
    public class ControllerUI : MonoUI
    {
        public Button m_LeftBtn;
        public Button m_RightBtn;
        public Button m_DownBtn;
        public Button m_UpBtn;
        public Button m_ConnectBtn;
        public TextMeshProUGUI m_IPText;

        private ClientView m_Client;
        private XFrameServer.Test.Entities.Game m_Game;
        private PlayerMoveComponent m_MoveComponent;

        protected override void OnInit()
        {
            base.OnInit();
            m_ConnectBtn.onClick.AddListener(InnerConnect);
            m_LeftBtn.onClick.AddListener(InnerLeft);
            m_RightBtn.onClick.AddListener(InnerRight);
            m_DownBtn.onClick.AddListener(InnerDown);
            m_UpBtn.onClick.AddListener(InnerUp);
            m_FixTimer = CDTimer.Create();
            m_FixTimer.Record(0.02f);
        }

        private bool m_Draging;
        private Vector3 m_LastPos;
        private CDTimer m_FixTimer;

        protected override void OnUpdate(double elapseTime)
        {
            base.OnUpdate(elapseTime);
            if (m_Client == null)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (!m_Draging)
                {
                    m_Draging = true;
                    m_LastPos = Input.mousePosition;
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (m_Draging)
                {
                    Vector3 curPos = Input.mousePosition;
                    Vector3 target = Camera.main.ScreenToWorldPoint(curPos) - Camera.main.ScreenToWorldPoint(m_LastPos);

                    if (target != Vector3.zero)
                    {
                        m_MoveComponent.Translate(target.ToStandardPos());
                    }
                    m_LastPos = Input.mousePosition;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_Draging = false;
            }
        }

        public void Bind(PlayerMoveComponent movement, ClientView client)
        {
            m_MoveComponent = movement;
            m_Client = client;
            m_LastPos = m_Client.Transform.position;
        }

        private void InnerConnect()
        {
            m_Game = Entry.GetModule<IEntityModule>().Create<XFrameServer.Test.Entities.Game>();
            m_Game.AddHandler<CreateEntityMessageHandler>();
            Entry.GetModule<NetworkModule>().Create(m_Game, NetMode.Client, IPAddress.Parse(m_IPText.text), 9999, XProtoType.Tcp);
        }

        private void InnerLeft()
        {
            m_MoveComponent?.Left();
        }

        private void InnerRight()
        {
            m_MoveComponent?.Right();
        }

        private void InnerDown()
        {
            m_MoveComponent?.Down();
        }

        private void InnerUp()
        {
            m_MoveComponent?.Up();
        }
    }
}

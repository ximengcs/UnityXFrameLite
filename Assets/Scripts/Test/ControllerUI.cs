using Assets.Scripts.Entities;
using System.Net;
using TMPro;
using UnityEngine.UI;
using UnityXFrame.Core.UIElements;
using XFrame.Core;
using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Test
{
    public class ControllerUI : MonoUI
    {
        public Button m_LeftBtn;
        public Button m_RightBtn;
        public Button m_DownBtn;
        public Button m_UpBtn;
        public Button m_ConnectBtn;
        public TextMeshProUGUI m_IPText;

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
        }

        public void Bind(PlayerMoveComponent movement)
        {
            m_MoveComponent = movement;
        }

        private void InnerConnect()
        {
            m_Game = Entry.GetModule<IEntityModule>().Create<XFrameServer.Test.Entities.Game>();
            m_Game.AddCom<CreateEntityMessageHandler>();
            Entry.GetModule<NetworkModule>().Create(m_Game, NetMode.Client, IPAddress.Parse(m_IPText.text), 9999);
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

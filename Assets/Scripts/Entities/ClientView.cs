using UnityEngine;
using XFrameShare.Network;
using Assets.Scripts.Test;
using XFrame.Modules.Entities;
using XFrame.Core;
using UnityXFrame.Core.Diagnotics;

namespace Assets.Scripts.Entities
{
    [NetEntityComponent(typeof(Client))]
    public class ClientView : Entity, INetEntityComponent
    {
        private Client m_Client;
        private IMailBox m_ClientMail;
        private GameObject m_Go;
        private SpriteRenderer m_Render;

        public Transform Transform => m_Go.transform;

        public bool IsSelf
        {
            get
            {
                return m_Client.GetCom<MailBoxCom>().Id == m_Client.Master.GetCom<ServerMailBoxCom>().ConnectEntity;
            }
        }

        protected override void OnInit()
        {
            base.OnInit();
            m_Client = Parent as Client;
            m_Go = new GameObject(m_Client.Name());
            Transform.localScale = Vector3.one * 0.5f;
            m_Render = m_Go.AddComponent<SpriteRenderer>();
            InnerInit();

            PlayerMoveComponent movement = m_Client.AddHandler<PlayerMoveComponent>(true);
            m_Client.AddFactory(movement);

            if (IsSelf)
            {
                Global.UI.Get<ControllerUI>().Bind(movement, this);
                Entry.GetModule<Debugger>().AddTitleShower(InnerShowPing);
            }

            m_Client.AddHandler<DestroyEntityMessageHandler>();
            m_ClientMail = m_Client.GetCom<IMailBox>(default, false);
        }

        private string InnerShowPing()
        {
            return $"{m_ClientMail.Ping} MS";
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Entry.GetModule<Debugger>().RemoveTitleShower(InnerShowPing);
            GameObject.Destroy(m_Go);
            m_Go = null;
        }

        private async void InnerInit()
        {
            m_Render.sprite = await Global.Res.LoadAsync<Sprite>("Data2/Textures/QQQ/white.png");
            m_Render.color = new Color[]
            {
                Color.cyan, Color.magenta, Color.red, Color.green, Color.blue, Color.yellow
            }[Random.Range(0, 6)];
            m_Render.sortingOrder = 1;
        }
    }
}

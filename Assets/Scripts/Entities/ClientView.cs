using UnityEngine;
using XFrameShare.Network;
using Assets.Scripts.Test;
using XFrame.Modules.Entities;
using System.Threading;

namespace Assets.Scripts.Entities
{
    [NetEntityComponent(typeof(Client))]
    public class ClientView : Entity, INetEntityComponent
    {
        private Client m_Client;
        private GameObject m_Go;
        private SpriteRenderer m_Render;

        public Transform Transform => m_Go.transform;

        protected override void OnInit()
        {
            base.OnInit();
            m_Client = Parent as Client;
            m_Go = new GameObject(m_Client.Name());
            Transform.localScale = Vector3.one * 0.5f;
            m_Render = m_Go.AddComponent<SpriteRenderer>();
            InnerInit();

            PlayerMoveComponent movement = m_Client.AddFactory<PlayerMoveComponent>();
            if (m_Client.GetCom<MailBoxCom>().Id == m_Client.Master.GetCom<ServerMailBoxCom>().ConnectEntity)
            {
                Global.UI.Get<ControllerUI>().Bind(movement);
            }

            m_Client.AddHandler<DestroyEntityMessageHandler>();
            m_Client.AddHandler<TransformMessageHandler>(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
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

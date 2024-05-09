using Assets.Scripts.Test;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Entities;
using XFrame.Modules.Pools;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    [NetEntityComponent(typeof(Player))]
    public class PlayerView : PoolObjectBase, INetEntityComponent
    {
        private Player m_Player;
        private GameObject m_Go;
        private SpriteRenderer m_Render;

        public Transform Transform => m_Go.transform;

        public void OnInit(IEntity entity)
        {
            m_Player = entity as Player;
            PlayerMoveComponent movement = m_Player.AddCom<PlayerMoveComponent>();

            if (m_Player.GetCom<MailBoxCom>().Id == m_Player.Master.GetCom<ServerMailBoxCom>().ConnectEntity)
            {
                Global.UI.Get<ControllerUI>().Bind(movement);
            }

            m_Player.AddCom<TransformMessageHandler>().Bind(this);
            InnerInit();
        }

        private async void InnerInit()
        {
            m_Go = new GameObject(m_Player.Id.ToString());
            Transform.localScale = Vector3.one * 0.5f;
            m_Render = m_Go.AddComponent<SpriteRenderer>();
            m_Render.sprite = await Global.Res.LoadAsync<Sprite>("Data2/Textures/QQQ/white.png");
            m_Render.color = new Color[]
            {
                Color.cyan, Color.magenta, Color.red, Color.green, Color.blue, Color.yellow
            }[Random.Range(0, 6)];
        }
    }
}

using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Entities;
using XFrame.Modules.Pools;
using XFrameShare.Core.Network;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    [NetEntityComponent(typeof(Player))]
    public class PlayerView : PoolObjectBase, INetEntityComponent
    {
        private Player m_Player;
        private GameObject m_Go;
        private PlayerMoveComponent m_Movement;
        private SpriteRenderer m_Render;

        public void OnInit(IEntity entity)
        {
            m_Player = entity as Player;
            m_Movement = m_Player.AddCom<PlayerMoveComponent>();
            InnerInit();
        }

        private async void InnerInit()
        {
            m_Go = new GameObject(m_Player.Id.ToString());
            m_Render = m_Go.AddComponent<SpriteRenderer>();
            m_Render.sprite = await Global.Res.LoadAsync<Sprite>("Data2/Textures/QQQ/white.png");
        }
    }
}

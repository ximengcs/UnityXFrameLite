using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Entities;
using XFrame.Modules.Pools;
using XFrameShare.Network;
using XFrameShare.Test;

namespace Assets.Scripts.Entities
{
    [NetEntityComponent(typeof(World))]
    public class WorldView : PoolObjectBase, INetEntityComponent
    {
        private World m_World;
        private GameObject m_Go;
        private SpriteRenderer m_Render;

        public void OnInit(IEntity entity)
        {
            m_World = entity as World;
            InnerInit();
        }

        private async void InnerInit()
        {
            m_Go = new GameObject(m_World.Id.ToString());
            m_Render = m_Go.AddComponent<SpriteRenderer>();
            //m_Render.sprite = await Global.Res.LoadAsync<Sprite>("Data2/Textures/QQQ/game_map.jpg");
        }
    }
}

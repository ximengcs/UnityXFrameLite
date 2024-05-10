using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Entities;
using XFrame.Modules.Pools;
using XFrameShare.Network;
using XFrameShare.Test;

namespace Assets.Scripts.Entities
{
    [NetEntityComponent(typeof(World))]
    public class WorldView : Entity, INetEntityComponent
    {
        private World m_World;
        private GameObject m_Go;
        private SpriteRenderer m_Render;

        protected override void OnInit()
        {
            base.OnInit();
            m_World = Parent as World;
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

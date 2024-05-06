using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Entities;
using XFrameShare.Test;

namespace Assets.Scripts.Entities
{
    [EntityView(typeof(World))]
    public class WorldView : MonoBehaviour, IEntityViewer
    {
        private World m_World;
        private SpriteRenderer m_Render;

        public void OnInit(IEntity entity)
        {
            m_World = entity as World;
            InnerInit();
        }

        private async void InnerInit()
        {
            m_Render = gameObject.AddComponent<SpriteRenderer>();
            m_Render.sprite = await Global.Res.LoadAsync<Sprite>("Data2/Textures/QQQ/game_map.jpg");
        }
    }
}

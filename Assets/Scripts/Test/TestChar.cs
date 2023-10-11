using UnityEngine;
using UnityXFrame.Core.Entities;
using UnityXFrame.Core.UIElements;
using XFrame.Modules.Entities;

namespace Game.Test
{
    public class TestChar : Entity
    {
        protected override void OnInit()
        {
            base.OnInit();
            GetOrAddCom<GameObjectCom>();
            GetOrAddCom<SceneUICom>((db) => db.SetData(Camera.main));
        }
    }
}

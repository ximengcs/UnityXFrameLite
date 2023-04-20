using UnityEngine;
using XFrame.Modules.Entities;

namespace UnityXFrame.Core.Entities
{
    public class GameObjectCom : EntityCom
    {
        private GameObject m_Inst;

        public Transform Tf => m_Inst.transform;

        protected override void OnInit()
        {
            base.OnInit();
            m_Inst = new GameObject();
        }
    }
}

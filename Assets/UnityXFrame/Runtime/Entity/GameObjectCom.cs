using UnityEngine;
using XFrame.Modules.Entities;
using XFrame.Utility;

namespace UnityXFrame.Core.Entities
{
    public class GameObjectCom : Entity
    {
        private GameObject m_Inst;

        public GameObject Inst => m_Inst;
        public Transform Tf => m_Inst.transform;

        protected override void OnInit()
        {
            base.OnInit();
            string name = GetData<string>();
            if (string.IsNullOrEmpty(name))
                name = TypeUtility.GetSimpleName(Master.GetType());
            m_Inst = new GameObject(name);
        }

        public GameObject AddChild(string name)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(Tf);
            return obj;
        }
    }
}

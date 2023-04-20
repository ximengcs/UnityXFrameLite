using UnityEngine;
using UnityEngine.EventSystems;
using XFrame.Modules.Containers;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIs
{
    public class UICommonCom : ShareCom
    {
        private IUI m_Inst;
        private const string UI_TAG = "UIComponent";
        private Dictionary<string, UIBehaviour> m_Coms;

        protected override void OnInit()
        {
            base.OnInit();
            m_Coms = new Dictionary<string, UIBehaviour>();
            m_Inst = (IUI)Master;
            InnerFindUIComponent(m_Inst.Root);
        }

        public void Add(string name)
        {
            UIBehaviour ui = InnerFindUIComponent(m_Inst.Root, name);
            if (ui != null)
                m_Coms.Add(name, ui);
        }

        public void Add(string name, UIBehaviour ui)
        {
            if (string.IsNullOrEmpty(name) || ui == null)
                return;
            m_Coms.Add(name, ui);
        }

        public T Get<T>(string name) where T : UIBehaviour
        {
            if (m_Coms.TryGetValue(name, out UIBehaviour ui))
                return (T)ui;
            else
                return default;
        }

        private void InnerFindUIComponent(Transform tf)
        {
            foreach (Transform child in tf)
            {
                if (child.tag == UI_TAG)
                    Add(child.name, child.GetComponent<UIBehaviour>());
                InnerFindUIComponent(child);
            }
        }

        private UIBehaviour InnerFindUIComponent(Transform tf, string name)
        {
            foreach (Transform child in tf)
            {
                if (child.name == name)
                    return child.GetComponent<UIBehaviour>();
                UIBehaviour ui = InnerFindUIComponent(child, name);
                if (ui != null)
                    return ui;
            }
            return default;
        }
    }
}

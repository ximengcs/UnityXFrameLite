using UnityEngine;
using XFrame.Modules.Containers;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIs
{
    public class UIFinder : ShareCom
    {
        private IUI m_Inst;
        private const string UI_TAG = "UIComponent";
        private Dictionary<string, RectTransform> m_Coms;

        protected override void OnInit()
        {
            base.OnInit();
            m_Coms = new Dictionary<string, RectTransform>();
            m_Inst = (IUI)Master;
            InnerFindUIComponent(m_Inst.Root);
        }

        public T GetUI<T>(string name) where T : Component
        {
            if (m_Coms.TryGetValue(name, out RectTransform tf))
                return tf.GetComponent<T>();
            else
                return default;
        }

        private void InnerFindUIComponent(Transform tf)
        {
            InnerCheckTf(tf);
            foreach (Transform child in tf)
                InnerFindUIComponent(child);
        }

        private void InnerCheckTf(Transform tf)
        {
            if (tf.tag == UI_TAG)
                m_Coms.Add(tf.name, tf.GetComponent<RectTransform>());
        }
    }
}

using UnityEngine;
using XFrame.Modules.Entities;

namespace UnityXFrame.Core.SceneUIs
{
    public abstract class SceneUIGroupItem : SceneUI
    {
        protected int m_Index;

        public void OnInit(int index, RectTransform tf, Entity entity)
        {
            m_Index = index;
            OnInit(entity, tf);
            Open();
        }
    }
}

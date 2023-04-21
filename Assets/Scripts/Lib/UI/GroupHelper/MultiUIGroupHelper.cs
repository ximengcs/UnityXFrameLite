using UnityXFrame.Core.UIs;
using System.Collections.Generic;

namespace UnityXFrameLib.UI
{
    /// <summary>
    /// 多UI可打开组辅助器
    /// </summary>
    public class MultiUIGroupHelper : UIGroupHelperInEffect
    {
        private int m_CurLayer;
        private HashSet<int> m_Opening;
        private HashSet<int> m_Closing;

        public MultiUIGroupHelper(IUIGroupHelperEffect openEffect, IUIGroupHelperEffect closeEffect) : base(openEffect, closeEffect)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();
            m_CurLayer = 0;
            m_Opening = new HashSet<int>();
            m_Closing = new HashSet<int>();
        }

        protected override void OnUIOpen(IUI ui)
        {
            int key = ui.GetHashCode();
            if (m_Closing.Contains(key))
            {
                m_CloseEffect.Kill(ui);
                m_Closing.Remove(key);
            }

            m_CurLayer++;
            ui.Layer = m_CurLayer;
            m_Opening.Add(key);
            InnerSetUIActive(ui, true);
            m_OpenEffect.Do(ui, () =>
            {
                if (m_Opening.Contains(key))
                    InnerOpenUI(ui);
            });
        }

        protected override void OnUIClose(IUI ui)
        {
            int key = ui.GetHashCode();
            if (m_Opening.Contains(key))
            {
                m_OpenEffect.Kill(ui);
                m_Opening.Remove(key);
            }

            m_CurLayer--;
            m_Closing.Add(key);
            m_CloseEffect.Do(ui, () =>
            {
                if (m_Closing.Contains(key))
                {
                    InnerCloseUI(ui);
                    m_Closing.Remove(key);
                    InnerSetUIActive(ui, false);
                }
            });
        }
    }
}

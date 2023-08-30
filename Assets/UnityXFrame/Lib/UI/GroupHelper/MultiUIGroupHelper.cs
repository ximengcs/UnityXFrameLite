using UnityXFrame.Core.UIElements;
using System.Collections.Generic;

namespace UnityXFrameLib.UIElements
{
    /// <summary>
    /// 多UI可打开组辅助器
    /// </summary>
    public class MultiUIGroupHelper : UIGroupHelperInEffect
    {
        private HashSet<int> m_Opening;
        private HashSet<int> m_Closing;

        protected override void OnInit()
        {
            base.OnInit();
            m_Opening = new HashSet<int>();
            m_Closing = new HashSet<int>();
        }

        protected override void OnUIOpen(IUI ui)
        {
            int key = ui.GetHashCode();
            if (m_Closing.Contains(key))
            {
                KillClose(ui);
                m_Closing.Remove(key);
            }

            ui.Layer = ui.Group.Count + 1;
            m_Opening.Add(key);
            InnerSetUIActive(ui, true);
            DoOpen(ui, () =>
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
                KillOpen(ui);
                m_Opening.Remove(key);
            }

            m_Closing.Add(key);
            DoClose(ui, () =>
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

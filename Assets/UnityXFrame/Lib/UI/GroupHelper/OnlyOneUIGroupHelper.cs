using UnityEngine;
using UnityXFrame.Core.UIElements;

namespace UnityXFrameLib.UIElements
{
    /// <summary>
    /// 单UI可打开组辅助器
    /// </summary>
    public class OnlyOneUIGroupHelper : UIGroupHelperInEffect
    {
        private IUI m_Opening;
        private IUI m_CurOpenUI;
        private bool m_IsOpen;

        protected override void OnInit()
        {
            base.OnInit();
            m_CurOpenUI = null;
            m_Opening = null;
            m_IsOpen = false;
        }

        protected override void OnUIOpen(IUI ui)
        {
            KillClose(ui);
            m_Opening?.Close();
            if (m_CurOpenUI != ui)
                m_CurOpenUI?.Close();
            m_CurOpenUI = null;

            m_Opening = ui;
            InnerSetUIActive(ui, true);
            DoOpen(ui, () =>
            {
                m_Opening = null;
                m_CurOpenUI = ui;
                InnerOpenUI(m_CurOpenUI);
                if (!m_IsOpen)
                {
                    m_IsOpen = true;
                }
            });
        }

        protected override void OnUIClose(IUI ui)
        {
            KillClose(ui);
            if (m_Opening != null)
            {
                KillOpen(m_Opening);
                m_Opening = null;
            }
            if (m_CurOpenUI != null && m_CurOpenUI != ui)
            {
                KillOpen(m_CurOpenUI);
                m_CurOpenUI = null;
            }

            DoClose(ui, () =>
            {
                InnerSetUIActive(ui, false);
                InnerCloseUI(ui);
                if (m_CurOpenUI == ui)
                {
                    if (m_IsOpen)
                    {
                        m_IsOpen = false;
                    }
                    m_CurOpenUI = null;
                }
            });
        }
    }
}

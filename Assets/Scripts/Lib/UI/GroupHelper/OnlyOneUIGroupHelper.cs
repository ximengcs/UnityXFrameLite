using UnityXFrame.Core.UIs;

namespace UnityXFrameLib.UI
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
            m_CloseEffect.Kill(ui);
            m_Opening?.Close();
            m_CurOpenUI?.Close();

            m_Opening = ui;
            InnerSetUIActive(ui, true);
            m_OpenEffect.Do(ui, () =>
            {
                m_Opening = null;
                m_CurOpenUI = ui;
                if (!m_IsOpen)
                {
                    InnerOpenUI(m_CurOpenUI);
                    m_IsOpen = true;
                }
            });
        }

        protected override void OnUIClose(IUI ui)
        {
            m_CloseEffect.Kill(ui);
            if (m_Opening != null)
            {
                m_OpenEffect.Kill(m_Opening);
                m_Opening = null;
            }
            if (m_CurOpenUI != null && m_CurOpenUI != ui)
            {
                m_OpenEffect.Kill(m_CurOpenUI);
                m_CurOpenUI = null;
            }

            m_CloseEffect.Do(ui, () =>
            {
                InnerSetUIActive(ui, false);
                if (m_CurOpenUI == ui)
                {
                    if (m_IsOpen)
                    {
                        InnerCloseUI(ui);
                        m_IsOpen = false;
                    }
                    m_CurOpenUI = null;
                }
            });
        }
    }
}

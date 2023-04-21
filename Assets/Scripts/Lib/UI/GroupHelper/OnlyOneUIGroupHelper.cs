using UnityXFrame.Core.UIs;

namespace UnityXFrameLib.UI
{
    /// <summary>
    /// 单UI可打开组辅助器
    /// </summary>
    public class OnlyOneUIGroupHelper : UIGroupHelperInEffect
    {
        private IUI m_CurOpenUI;

        protected override void OnInit()
        {
            base.OnInit();
            m_CurOpenUI = null;
        }

        protected override void OnUIOpen(IUI ui)
        {
            m_OpenEffect.Kill(ui);
            m_CurOpenUI?.Close();
            m_CurOpenUI = ui;

            InnerSetUIActive(ui, true);
            m_OpenEffect.Do(ui, () =>
            {
                if (m_CurOpenUI == ui)
                    InnerOpenUI(m_CurOpenUI);
            });
        }

        protected override void OnUIClose(IUI ui)
        {
            m_CloseEffect.Kill(ui);
            m_CloseEffect.Do(ui, () =>
            {
                if (m_CurOpenUI == ui)
                {
                    InnerCloseUI(m_CurOpenUI);
                    InnerSetUIActive(m_CurOpenUI, false);
                    m_CurOpenUI = null;
                }
            });
        }
    }
}

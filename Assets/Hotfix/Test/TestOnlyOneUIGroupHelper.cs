using DG.Tweening;
using UnityEngine;
using UnityXFrame.Core.UIs;

namespace XHotfix.Test
{
    public class TestOnlyOneUIGroupHelper : UIGroupHelperBase
    {
        private IUI m_CurOpenUI;
        private Vector3 m_DefaultScale;
        private Tween m_OpenAnim;
        private Tween m_CloseAnim;

        protected override void OnInit()
        {
            base.OnInit();
            m_DefaultScale = Vector2.one * 0;
        }

        protected override void OnUIOpen(IUI ui)
        {
            m_OpenAnim?.Kill();
            m_CurOpenUI?.Close();
            m_CurOpenUI = ui;

            InnerSetUIActive(ui, true);
            if (ui.Root.localScale == Vector3.one)
                ui.Root.localScale = m_DefaultScale;
            m_OpenAnim = ui.Root.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                if (m_CurOpenUI == ui)
                {
                    InnerOpenUI(m_CurOpenUI);
                    m_OpenAnim = null;
                }
            });
        }

        protected override void OnUIClose(IUI ui)
        {
            m_CloseAnim?.Kill();
            m_CloseAnim = ui.Root.DOScale(m_DefaultScale, 0.3f).OnComplete(() =>
            {
                if (m_CurOpenUI == ui)
                {
                    InnerCloseUI(m_CurOpenUI);
                    InnerSetUIActive(m_CurOpenUI, false);
                    m_CurOpenUI = null;
                    m_CloseAnim = null;
                }
            });
        }
    }
}

using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIs
{
    public class TestMainGroupHelper : UIGroupHelperBase
    {
        private Dictionary<int, Tween> m_Opening;
        private Dictionary<int, Tween> m_Closing;

        protected override void OnInit()
        {
            base.OnInit();
            m_Opening = new Dictionary<int, Tween>();
            m_Closing = new Dictionary<int, Tween>();
        }

        protected override void OnUIOpen(IUI ui)
        {
            int key = ui.GetHashCode();
            if (m_Closing.TryGetValue(key, out Tween tween))
            {
                tween?.Kill();
                m_Closing.Remove(key);
            }

            InnerSetUIActive(ui, true);
            if (ui.Root.anchoredPosition == Vector2.zero)
            {
                Vector2 startPos = new Vector2(-ui.Root.sizeDelta.x, 0);
                ui.Root.anchoredPosition = startPos;
            }
            
            tween = DOTween.To(
                () => ui.Root.anchoredPosition,
                (pos) => ui.Root.anchoredPosition = pos,
                Vector2.zero, 0.3f).OnComplete(() =>
                {
                    InnerOpenUI(ui);
                    m_Opening.Remove(key);
                });
            m_Opening.Add(key, tween);
        }

        protected override void OnUIClose(IUI ui)
        {
            int key = ui.GetHashCode();
            if (m_Opening.TryGetValue(key, out Tween tween))
            {
                tween?.Kill();
                m_Opening.Remove(key);
            }

            Vector2 startPos = new Vector2(-ui.Root.sizeDelta.x, 0);
            tween = DOTween.To(
                () => ui.Root.anchoredPosition,
                (pos) => ui.Root.anchoredPosition = pos,
                startPos, 0.3f).OnComplete(() =>
                {
                    InnerCloseUI(ui);
                    InnerSetUIActive(ui, false);
                    m_Closing.Remove(key);
                });
            m_Closing.Add(key, tween);
        }
    }
}

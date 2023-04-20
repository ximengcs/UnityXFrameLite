using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIs
{
    public class TestUIGroupHelper : UIGroupHelperBase
    {
        private Dictionary<int, Tween> m_Opening;
        private Dictionary<int, Tween> m_Closing;
        private Vector3 m_DefaultScale;
        private int m_CurLayer;

        protected override void OnInit()
        {
            base.OnInit();
            m_CurLayer = 0;
            m_DefaultScale = Vector2.one * 0;
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

            m_CurLayer++;
            ui.Layer = m_CurLayer;
            ui.Root.localScale = m_DefaultScale;
            tween = ui.Root.DOScale(Vector3.one, 0.3f).OnComplete(() =>
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

            m_CurLayer--;
            tween = ui.Root.DOScale(m_DefaultScale, 0.3f).OnComplete(() =>
            {
                InnerCloseUI(ui);
                InnerSetUIActive(ui, false);
                m_Closing.Remove(key);
            });
            m_Closing.Add(key, tween);
        }
    }
}

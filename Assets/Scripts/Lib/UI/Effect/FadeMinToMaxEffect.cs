using System;
using DG.Tweening;
using UnityEngine;
using UnityXFrame.Core.UIs;
using System.Collections.Generic;

namespace UnityXFrameLib.UI
{
    /// <summary>
    /// 渐隐渐显效果
    /// </summary>
    public class FadeMinToMaxEffect : IUIGroupHelperEffect
    {
        private float m_Target;
        private Dictionary<int, Tween> m_Anims;

        public FadeMinToMaxEffect(float target)
        {
            m_Target = target;
            m_Anims = new Dictionary<int, Tween>();
        }

        public void Do(IUI ui, Action onComplete)
        {
            int key = ui.GetHashCode();
            CanvasGroup canvasGroup = InnerEnsureCanvasGroup(ui);
            m_Anims.Add(key, canvasGroup.DOAlpha(m_Target, 0.3f).OnComplete(() =>
            {
                onComplete?.Invoke();
                m_Anims.Remove(key);
            }));
        }

        public void Kill(IUI ui)
        {
            int key = ui.GetHashCode();
            if (m_Anims.TryGetValue(key, out Tween tween))
            {
                tween.Kill();
                m_Anims.Remove(key);
            }
        }

        private CanvasGroup InnerEnsureCanvasGroup(IUI ui)
        {
            CanvasGroup canvasGroup = ui.Root.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = ui.Root.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            return canvasGroup;
        }
    }
}

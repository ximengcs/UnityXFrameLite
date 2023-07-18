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
    public class FadeEffect : IUIGroupHelperEffect
    {
        private float m_Dur;
        private float m_Start;
        private float m_Target;
        private Dictionary<int, Tween> m_Anims;

        public FadeEffect(float target, float duration = 0.2f)
        {
            m_Start = 0;
            m_Target = target;
            m_Dur = duration;
            m_Anims = new Dictionary<int, Tween>();
        }

        public FadeEffect(float start, float target, float duration = 0.2f)
        {
            m_Start = start;
            m_Target = target;
            m_Dur = duration;
            m_Anims = new Dictionary<int, Tween>();
        }

        void IUIGroupHelperEffect.OnUpdate()
        {

        }

        public bool Do(IUI ui, Action onComplete)
        {
            int key = ui.GetHashCode();
            CanvasGroup canvasGroup = InnerEnsureCanvasGroup(ui);
            m_Anims.Add(key, canvasGroup.DOAlpha(m_Target, m_Dur).OnComplete(() =>
            {
                onComplete?.Invoke();
                m_Anims.Remove(key);
            }));
            return true;
        }

        public bool Kill(IUI ui)
        {
            int key = ui.GetHashCode();
            if (m_Anims.TryGetValue(key, out Tween tween))
            {
                tween.Kill();
                m_Anims.Remove(key);
            }
            return true;
        }

        private CanvasGroup InnerEnsureCanvasGroup(IUI ui)
        {
            CanvasGroup canvasGroup = ui.Root.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = ui.Root.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = m_Start;
            return canvasGroup;
        }
    }
}

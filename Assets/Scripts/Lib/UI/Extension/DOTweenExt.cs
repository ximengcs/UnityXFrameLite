using DG.Tweening;
using UnityEngine;

namespace UnityXFrameLib.UI
{
    public static class DOTweenExt
    {
        public static Tween DOAnchoredPos(this RectTransform tf, Vector2 target, float duration)
        {
            return DOTween.To(() => tf.anchoredPosition, (pos) => tf.anchoredPosition = pos, target, duration);
        }

        public static Tween DOAlpha(this CanvasGroup group, float target, float duration)
        {
            return DOTween.To(() => group.alpha, (pos) => group.alpha = pos, target, duration);
        }
    }
}

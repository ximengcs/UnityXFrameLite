using System;
using DG.Tweening;
using UnityEngine;
using UnityXFrame.Core.UIs;
using System.Collections.Generic;

namespace UnityXFrameLib.UI
{
    /// <summary>
    /// 缩放效果
    /// </summary>
    public class ScaleEffect : IUIGroupHelperEffect
    {
        private Vector3 m_Target;
        private Dictionary<int, Tween> m_Anims;

        public ScaleEffect(Vector2 target)
        {
            m_Target = target;
            m_Target.z = 1;
            m_Anims = new Dictionary<int, Tween>();
        }

        public void Do(IUI ui, Action onComplete)
        {
            int key = ui.GetHashCode();
            m_Anims.Add(key, ui.Root.DOScale(m_Target, 0.3f).OnComplete(() =>
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
    }
}

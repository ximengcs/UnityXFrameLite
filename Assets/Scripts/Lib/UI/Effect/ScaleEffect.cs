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
        private float m_Dur;
        private Vector3 m_Start;
        private Vector3 m_Target;
        private Dictionary<int, Tween> m_Anims;

        public ScaleEffect(Vector2 target, float duration = 0.2f)
        {
            m_Dur = duration;
            m_Start = new Vector3(0, 0, 1);
            m_Target = target;
            m_Target.z = m_Start.z;
            m_Anims = new Dictionary<int, Tween>();
        }

        public ScaleEffect(Vector2 start, Vector2 target, float duration = 0.2f)
        {
            m_Dur = duration;
            m_Start = start;
            m_Target = target;
            m_Start.z = 1;
            m_Target.z = m_Start.z;
            m_Anims = new Dictionary<int, Tween>();
        }

        public void Do(IUI ui, Action onComplete)
        {
            int key = ui.GetHashCode();
            if (ui.Root.localScale == m_Target)
                ui.Root.localScale = m_Start;
            m_Anims.Add(key, ui.Root.DOScale(m_Target, m_Dur).OnComplete(() =>
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

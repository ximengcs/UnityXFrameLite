using System;
using DG.Tweening;
using UnityEngine;
using XFrame.Module.Rand;
using UnityXFrame.Core.UIs;
using System.Collections.Generic;

namespace UnityXFrameLib.UI
{
    /// <summary>
    /// 移动效果
    /// </summary>
    public class MoveEffect : IUIGroupHelperEffect
    {
        public enum Direct
        {
            FromTop,
            FromBottom,
            FromLeft,
            FromRight,
            Rand
        }

        private float m_Dur;
        private bool m_IsOpen;
        private Direct m_Direct;
        private bool m_CompleteReset;
        private Dictionary<int, Tween> m_Anims;

        public MoveEffect(Direct direct, bool open, bool completeRest, float duration = 0.2f)
        {
            m_IsOpen = open;
            m_Direct = direct;
            m_Dur = duration;
            m_CompleteReset = completeRest;
            m_Anims = new Dictionary<int, Tween>();
        }

        public void Do(IUI ui, Action onComplete)
        {
            int key = ui.GetHashCode();
            Vector2 start;
            Vector2 end;
            Direct direct = m_Direct;
            if (m_Direct == Direct.Rand)
                direct = RandModule.Inst.RandEnum(Direct.Rand);
            switch (direct)
            {
                case Direct.FromLeft:
                    start = new Vector2(-ui.Root.sizeDelta.x, 0);
                    end = Vector2.zero;
                    break;

                case Direct.FromRight:
                    start = new Vector2(ui.Root.sizeDelta.x, 0);
                    end = Vector2.zero;
                    break;

                case Direct.FromTop:
                    start = new Vector2(0, ui.Root.sizeDelta.y);
                    end = Vector2.zero;
                    break;

                case Direct.FromBottom:
                    start = new Vector2(0, -ui.Root.sizeDelta.y);
                    end = Vector2.zero;
                    break;

                default:
                    start = Vector2.zero;
                    end = Vector2.zero;
                    break;
            }

            if (!m_IsOpen)
            {
                Vector2 tmp = start;
                start = end;
                end = tmp;
            }

            ui.Root.anchoredPosition = start;
            Tween tween = ui.Root.DOAnchoredPos(end, m_Dur);
            tween.OnComplete(() =>
            {
                onComplete?.Invoke();
                m_Anims.Remove(key);
            });
            if (m_CompleteReset)
            {
                tween.OnKill(() =>
                {
                    ui.Root.anchoredPosition = start;
                });
            }
            m_Anims.Add(key, tween);
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

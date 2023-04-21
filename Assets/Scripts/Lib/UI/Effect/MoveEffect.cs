using System;
using DG.Tweening;
using UnityEngine;
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

        private bool m_IsOpen;
        private Direct m_Direct;
        private Dictionary<int, Tween> m_Anims;

        public MoveEffect(Direct direct, bool open)
        {
            m_IsOpen = open;
            m_Direct = direct;
            m_Anims = new Dictionary<int, Tween>();
        }

        public void Do(IUI ui, Action onComplete)
        {
            int key = ui.GetHashCode();
            Vector2 start;
            Vector2 end;

            switch (m_Direct)
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
            m_Anims.Add(key, ui.Root.DOAnchoredPos(end, 0.3f).OnComplete(() =>
            {
                onComplete?.Invoke();
                ui.Root.anchoredPosition = start;
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

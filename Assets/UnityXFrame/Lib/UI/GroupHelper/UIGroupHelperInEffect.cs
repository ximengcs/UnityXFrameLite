using System;
using XFrame.Core;
using UnityXFrame.Core.UIs;
using System.Collections.Generic;

namespace UnityXFrameLib.UI
{
    /// <summary>
    /// 附带UI打开关闭效果的组辅助器
    /// </summary>
    public class UIGroupHelperInEffect : UIGroupHelperBase
    {
        private List<IUIGroupHelperEffect> m_OpenEffect;
        private List<IUIGroupHelperEffect> m_CloseEffect;

        protected override void OnInit()
        {
            base.OnInit();
            m_OpenEffect = new List<IUIGroupHelperEffect>(2) { null };
            m_CloseEffect = new List<IUIGroupHelperEffect>(2) { null };
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            foreach (IUIGroupHelperEffect effect in m_OpenEffect)
                effect.OnUpdate();
            foreach (IUIGroupHelperEffect effect in m_CloseEffect)
                effect.OnUpdate();
        }

        public void SetEffect(IUIGroupHelperEffect open, IUIGroupHelperEffect close)
        {
            m_OpenEffect[0] = open;
            m_CloseEffect[0] = close;
        }

        public void AddAlternateEffect(IUIGroupHelperEffect open, IUIGroupHelperEffect close)
        {
            m_OpenEffect.Add(open);
            m_CloseEffect.Add(close);
        }

        protected void DoOpen(IUI ui, Action onComplete)
        {
            foreach (IUIGroupHelperEffect effect in m_OpenEffect)
            {
                if (effect == null)
                    continue;
                if (effect.Do(ui, onComplete))
                    break;
            }
        }

        protected void DoClose(IUI ui, Action onComplete)
        {
            foreach (IUIGroupHelperEffect effect in m_CloseEffect)
            {
                if (effect == null)
                    continue;
                if (effect.Do(ui, onComplete))
                    break;
            }
        }

        protected void KillOpen(IUI ui)
        {
            foreach (IUIGroupHelperEffect effect in m_OpenEffect)
            {
                if (effect == null)
                    continue;
                if (effect.Kill(ui))
                    break;
            }
        }

        protected void KillClose(IUI ui)
        {
            foreach (IUIGroupHelperEffect effect in m_CloseEffect)
            {
                if (effect == null)
                    continue;
                if (effect.Kill(ui))
                    break;
            }
        }
    }
}

using System;
using UnityEngine;
using UnityXFrame.Core.UIs;
using UnityXFrameLib.Animations;
using XFrame.Modules.Diagnotics;

namespace UnityXFrameLib.UI
{
    public class AnimatorStateEffect : AnimatorChecker, IUIGroupHelperEffect
    {
        private int m_Layer;
        private string m_StateName;

        public AnimatorStateEffect(string stateName, int layer = 0)
        {
            m_StateName = stateName;
            m_Layer = layer;
        }

        void IUIGroupHelperEffect.OnUpdate()
        {
            UpdateState();
        }

        public bool Do(IUI ui, Action onComplete)
        {
            Animator animator = ui.Root.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(m_StateName);
                Add(animator, m_StateName, m_Layer, onComplete);
                return true;
            }
            else
            {
                Log.Debug("UI", $"UI {ui.Name} do not has animtor component, will use other effect");
                return false;
            }
        }

        public bool Kill(IUI ui)
        {
            Animator animator = ui.Root.GetComponent<Animator>();
            if (animator != null)
            {
                Remove(animator);
                return true;
            }
            else
            {
                Log.Debug("UI", $"UI {ui.Name} do not has animtor component, will use other effect");
                return false;
            }
        }
    }
}

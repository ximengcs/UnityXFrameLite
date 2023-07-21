using System;
using UnityEngine;
using UnityXFrame.Core.UIs;
using XFrame.Modules.Diagnotics;
using UnityXFrameLib.Animations;

namespace UnityXFrameLib.UI
{
    public class AnimatorTriggerEffect : AnimatorChecker, IUIGroupHelperEffect
    {
        private int m_Layer;
        private string m_Trigger;
        private string m_StateName;

        public AnimatorTriggerEffect(string trigger, int layer = 0)
        {
            m_Trigger = trigger;
            m_StateName = trigger;
            m_Layer = layer;
        }

        public AnimatorTriggerEffect(string trigger, string stateName, int layer = 0)
        {
            m_Trigger = trigger;
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
                animator.SetTrigger(m_Trigger);
                Add(animator, m_StateName, m_Layer, onComplete);
                return true;
            }
            else
            {
                Log.Debug("XFrame", $"UI {ui.Name} do not has animtor component, will use other effect");
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
                Log.Debug("XFrame", $"UI {ui.Name} do not has animtor component, will use other effect");
                return false;
            }
        }
    }
}

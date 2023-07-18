using System;
using UnityEngine;
using XFrame.Collections;
using UnityXFrame.Core.UIs;
using XFrame.Modules.Diagnotics;

namespace UnityXFrameLib.UI
{
    public class AnimatorEffect : IUIGroupHelperEffect
    {
        private struct CheckInfo
        {
            public string Name;
            public int Layer;
            public bool IsReady;
            public Action Callback;
            public Animator Anim;

            public CheckInfo(Animator anim, string name, int layer, Action callback)
            {
                Anim = anim;
                Name = name;
                Layer = layer;
                IsReady = false;
                Callback = callback;
            }
        }

        private int m_Layer;
        private string m_Trigger;
        private string m_StateName;
        private XLinkList<CheckInfo> m_Checks;

        public AnimatorEffect(string trigger, string stateName, int layer = 0)
        {
            m_Trigger = trigger;
            m_StateName = stateName;
            m_Layer = layer;
            m_Checks = new XLinkList<CheckInfo>();
        }

        void IUIGroupHelperEffect.OnUpdate()
        {
            foreach (XLinkNode<CheckInfo> node in m_Checks)
            {
                CheckInfo info = node.Value;
                AnimatorStateInfo state = info.Anim.GetCurrentAnimatorStateInfo(info.Layer);
                if (!info.IsReady)
                {
                    if (state.IsName(info.Name))
                    {
                        info.IsReady = true;
                        node.Value = info;
                    }
                }
                else
                {
                    if (!state.IsName(info.Name) || state.normalizedTime >= 1)
                    {
                        info.Callback?.Invoke();
                        node.Delete();
                    }
                }
            }
        }

        public bool Do(IUI ui, Action onComplete)
        {
            Animator animator = ui.Root.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(m_Trigger);
                m_Checks.AddLast(new CheckInfo(animator, m_StateName, m_Layer, onComplete));
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
                foreach (XLinkNode<CheckInfo> node in m_Checks)
                {
                    if (node.Value.Anim == animator)
                    {
                        node.Delete();
                        break;
                    }
                }
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

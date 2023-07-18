using System;
using UnityEngine;
using XFrame.Collections;

namespace UnityXFrameLib.Animations
{
    public partial class AnimatorChecker
    {
        private XLinkList<CheckInfo> m_Checks;

        public AnimatorChecker()
        {
            m_Checks = new XLinkList<CheckInfo>();
        }

        public void UpdateState()
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

        public void Add(Animator anim, string name, int layer, Action callback)
        {
            m_Checks.AddLast(new CheckInfo(anim, name, layer, callback));
        }

        public void Remove(Animator anim)
        {
            foreach (XLinkNode<CheckInfo> node in m_Checks)
            {
                if (node.Value.Anim == anim)
                {
                    node.Delete();
                    break;
                }
            }
        }

        public void Remove(Animator anim, string name)
        {
            foreach (XLinkNode<CheckInfo> node in m_Checks)
            {
                if (node.Value.Anim == anim && node.Value.Name == name)
                {
                    node.Delete();
                    break;
                }
            }
        }
    }
}

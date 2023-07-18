using System;
using UnityEngine;

namespace UnityXFrameLib.Animations
{
    public partial class AnimatorChecker
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
    }
}

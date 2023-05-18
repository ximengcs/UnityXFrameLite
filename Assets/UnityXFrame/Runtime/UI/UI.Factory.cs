using System;
using UnityEngine;
using UnityXFrame.Core.Resource;
using XFrame.Modules.Pools;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.UIs
{
    public partial class UI
    {
        internal class Factory : IUIFactory
        {
            public IUI Create(GameObject root, Type uiType)
            {
                return (IUI)Activator.CreateInstance(uiType);
            }
        }

    }
}

using System;
using UnityEngine;

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

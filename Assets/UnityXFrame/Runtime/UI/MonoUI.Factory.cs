using System;
using UnityEngine;

namespace UnityXFrame.Core.UIs
{
    public partial class MonoUI
    {
        internal class Factory : IUIFactory
        {
            public IUI Create(GameObject root, Type uiType)
            {
                IUI ui = (IUI)root.GetComponent(uiType);
                if (ui == null)
                    ui = (IUI)root.AddComponent(uiType);
                return ui;
            }
        }
    }
}

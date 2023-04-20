using System;
using UnityEngine;

namespace UnityXFrame.Core.UIs
{
    public interface IUIFactory
    {
        IUI Create(GameObject root, Type uiType);
    }
}

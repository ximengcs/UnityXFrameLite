using System;
using UnityEngine;

namespace UnityXFrame.Core.UIElements
{
    public interface IUIFactory
    {
        IUI Create(GameObject root, Type uiType);
    }
}

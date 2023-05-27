
using System;

namespace UnityXFrameLib.UI
{
    public class UIAutoAttribute : Attribute
    {
        public bool Native { get; }

        public UIAutoAttribute(bool native)
        {
            Native = native;
        }
    }
}

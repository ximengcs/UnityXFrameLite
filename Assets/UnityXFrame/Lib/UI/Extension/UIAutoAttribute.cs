
using System;

namespace UnityXFrameLib.UIElements
{
    public class UIAutoAttribute : Attribute
    {
        public int UseResModule { get; }

        public UIAutoAttribute(int useResModule)
        {
            useResModule = UseResModule;
        }
    }
}

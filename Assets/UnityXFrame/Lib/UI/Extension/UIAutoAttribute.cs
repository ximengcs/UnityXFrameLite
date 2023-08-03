
using System;

namespace UnityXFrameLib.UI
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

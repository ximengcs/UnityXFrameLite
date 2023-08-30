using System;
using UnityXFrame.Core;

namespace UnityXFrameLib.UIElements
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoLoadUIAttribute : UIAutoAttribute
    {
        public AutoLoadUIAttribute(int useResModule = Constant.COMMON_RES_MODULE) : base(useResModule) { }
    }
}

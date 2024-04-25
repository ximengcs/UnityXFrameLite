using System;
using UnityXFrame.Core;

namespace UnityXFrameLib.UIElements
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoSpwanUIAttribute : UIAutoAttribute
    {
        public int Count { get; }

        public AutoSpwanUIAttribute(int useResModule = Constant.COMMON_RES_MODULE, int count = 1) : base(useResModule)
        {
            Count = count;
        }
    }
}

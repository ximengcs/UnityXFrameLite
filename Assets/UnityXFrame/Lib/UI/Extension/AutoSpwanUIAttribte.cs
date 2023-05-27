using System;

namespace UnityXFrameLib.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoSpwanUIAttribute : UIAutoAttribute
    {
        public AutoSpwanUIAttribute(bool native) : base(native) { }
    }
}

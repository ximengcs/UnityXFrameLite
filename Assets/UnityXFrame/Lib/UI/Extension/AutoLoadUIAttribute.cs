using System;

namespace UnityXFrameLib.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoLoadUIAttribute : UIAutoAttribute
    {
        public AutoLoadUIAttribute(bool native = false) : base(native) { }
    }
}

using System;

namespace UnityXFrameLib.Commercial
{
    public class AdsImplementAttribute : Attribute
    {
        public int Type { get; }

        public AdsImplementAttribute(int adType)
        {
            Type = adType;
        }
    }
}

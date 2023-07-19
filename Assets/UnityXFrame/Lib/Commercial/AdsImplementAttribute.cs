using System;

namespace UnityXFrameLib.Commercial
{
    public class AdsImplementAttribute : Attribute
    {
        public AdType Type { get; }

        public AdsImplementAttribute(AdType adType)
        {
            Type = adType;
        }
    }
}

using UnityXFrameLib.Commercial;
using UnityXFrameLib.Improve;
using XFrame.Core;

namespace UnityXFrameLib
{
    public static partial class LibModule
    {
        public static IAdsModule Ads => m_Ads ??= Entry.GetModule<IAdsModule>();
        public static IGCModule GC => m_GC ??= Entry.GetModule<IGCModule>();
    }
}

using UnityXFrameLib.Commercial;
using UnityXFrameLib.Improve;
using XFrame.Core;

namespace UnityXFrameLib
{
    public static partial class LibModule
    {
        public static IAdsModule Ads => m_Ads != null ? m_Ads : m_Ads = Entry.GetModule<IAdsModule>();
        public static IGCModule GC => m_GC != null ? m_GC : m_GC = Entry.GetModule<IGCModule>();
    }
}

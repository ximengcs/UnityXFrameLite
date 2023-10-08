using UnityXFrameLib.Commercial;
using UnityXFrameLib.Improve;

namespace UnityXFrameLib
{
    public static partial class LibModule
    {
        private static IGCModule m_GC;
        private static IAdsModule m_Ads;

        public static void Refresh()
        {
            m_GC = null;
            m_Ads = null;
        }
    }
}

using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.UIElements;
using XFrame.Modules.Resource;

namespace XFrame.Core
{
    public static partial class XModule
    {
        private static IResModule m_LocalRes;
        private static IUIModule m_UI;
        private static IAudioModule m_Audio;
        private static IDebugger m_Debugger;

        public static void Refresh()
        {
            m_LocalRes = null;
            m_UI = null;
            m_Audio = null;
            m_Debugger = null;
        }
    }
}

using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.Resource;
using UnityXFrame.Core.UIElements;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core
{
    public static partial class Global
    {
        private static IResModule m_LocalRes;
        private static IUIModule m_UI;
        private static IAudioModule m_Audio;
        private static IDebugger m_Debugger;
        private static EndOfFrameModule m_EndOfFrame;
        private static ISpriteAtlasModule m_SpriteAtlas;

        public static void Refresh()
        {
            m_LocalRes = null;
            m_UI = null;
            m_Audio = null;
            m_Debugger = null;
            m_EndOfFrame = null;
        }
    }
}

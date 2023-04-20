#if CONSOLE
namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debuger
    {
        public static void Tip(IDebugWindow from, string content, string color = null)
        {
            Inst.SetTip(from, content, color);
        }
    }
}
#endif

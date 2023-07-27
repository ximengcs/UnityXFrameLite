using UnityEngine;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debuger
    {
        public static void Tip(IDebugWindow from, string content, string color = null)
        {
            Inst.SetTip(from, content, color);
        }

        public static void Tip(IDebugWindow from, string content, Color color)
        {
            string colorStr = null;
            if (color != default)
                colorStr = ColorUtility.ToHtmlStringRGB(color);
            Tip(from, content, colorStr);
        }

        public static void Tip(string content, string color = default)
        {
            Inst.SetTip(-1, content, color);
        }

        public static void Tip(string content, Color color)
        {
            string colorStr = null;
            if (color != default)
                colorStr = ColorUtility.ToHtmlStringRGB(color);
            Tip(content, colorStr);
        }
    }
}

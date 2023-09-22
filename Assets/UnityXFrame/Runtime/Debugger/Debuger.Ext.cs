using UnityEngine;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Debugger
    {
        public void SetTip(IDebugWindow from, string content, Color color)
        {
            string colorStr = null;
            if (color != default)
                colorStr = ColorUtility.ToHtmlStringRGB(color);
            SetTip(from, content, colorStr);
        }

        public void SetTip(string content, string color = default)
        {
            SetTip(-1, content, color);
        }

        public void SetTip(string content, Color color)
        {
            string colorStr = null;
            if (color != default)
                colorStr = ColorUtility.ToHtmlStringRGB(color);
            SetTip(content, colorStr);
        }
    }
}

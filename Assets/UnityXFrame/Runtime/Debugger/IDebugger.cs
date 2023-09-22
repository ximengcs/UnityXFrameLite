
using UnityEngine;
using XFrame.Core;

namespace UnityXFrame.Core.Diagnotics
{
    public interface IDebugger : IModule, IGUI
    {
        void SetTip(IDebugWindow from, string content, string color = null);
        void SetTip(int instanceId, string content, string color = null);
        void SetTip(IDebugWindow from, string content, Color color);
        void SetTip(string content, string color = default);
        void SetTip(string content, Color color);
        void SetCmdHelpInfo(string info);
        float FitWidth(float width);
    }
}

using UnityEngine;
using XFrame.Modules.Archives;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugCommandClass]
    internal class CmdList
    {
        [DebugCommand]
        public static void clear()
        {
            ArchiveModule.Inst.DeleteAll();
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
    }
}

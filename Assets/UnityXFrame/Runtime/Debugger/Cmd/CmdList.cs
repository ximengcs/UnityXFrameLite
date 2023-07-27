using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Archives;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugCommandClass]
    internal class CmdList
    {
        [DebugCommand]
        public static void clear(params string[] args)
        {
            ArchiveModule.Inst.DeleteAll();
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
    }
}

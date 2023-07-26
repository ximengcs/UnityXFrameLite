using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Archives;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugCommandClass]
    internal class CmdList : Singleton<CmdList>
    {
        [DebugCommand]
        public void clear(params string[] args)
        {
            ArchiveModule.Inst.DeleteAll();
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
    }
}

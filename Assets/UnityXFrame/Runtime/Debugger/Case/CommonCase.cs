using UnityEngine;
using XFrame.Modules.Archives;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugWindow(-996)]
    public class CommonCase : IDebugWindow
    {
        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Clear User Data"))
            {
                ArchiveModule.Inst.DeleteAll();
                PlayerPrefs.DeleteAll();
                Application.Quit();
            }
        }
    }
}
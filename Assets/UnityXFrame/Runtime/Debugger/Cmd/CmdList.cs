using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Archives;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugCommandClass]
    public class CmdList
    {
        [DebugCommand]
        public static void clear_user_data()
        {
            ArchiveModule.Inst.DeleteAll();
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }

        [DebugCommand]
        public static void clear()
        {
            Debuger.Inst.InnerClearCmd();
        }

        [DebugCommand]
        public static void close()
        {
            Debuger.Inst.InnerClose();
        }

        [DebugCommand]
        public static void collapse()
        {
            Debuger.Inst.InnerCollapse(1);
        }

        [DebugCommand]
        public static void expend()
        {
            Debuger.Inst.InnerCollapse(0);
        }

        [DebugCommand]
        public static void fps(string on)
        {
            bool open = true;
            if (!string.IsNullOrEmpty(on))
            {
                if (IntParser.TryParse(on, out int value))
                {
                    open = value != 0 ? true : false;
                }
                else
                {
                    if (on == "on")
                        open = true;
                    else
                        open = false;
                }
            }
            Debuger.Inst.InnerSwitchFPS(open);
        }
    }
}

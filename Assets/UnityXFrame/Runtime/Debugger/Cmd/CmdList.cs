using UnityEngine;
using XFrame.Core;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugCommandClass]
    public class CmdList
    {
        [DebugCommand]
        public static void clear_user_data()
        {
            XModule.Archive.DeleteAll();
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }

        [DebugCommand]
        public static void clear()
        {
            Debugger debugger = (Debugger)XModule.Debugger;
            debugger.InnerClearCmd();
        }

        [DebugCommand]
        public static void close()
        {
            Debugger debugger = (Debugger)XModule.Debugger;
            debugger.InnerClose();
        }

        [DebugCommand]
        public static void collapse()
        {
            Debugger debugger = (Debugger)XModule.Debugger;
            debugger.InnerCollapse(1);
        }

        [DebugCommand]
        public static void expend()
        {
            Debugger debugger = (Debugger)XModule.Debugger;
            debugger.InnerCollapse(0);
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
            Debugger debugger = (Debugger)XModule.Debugger;
            debugger.InnerSwitchFPS(open);
        }
    }
}

using System;
using UnityEngine;
using UnityXFrame.Core.UIElements;
using XFrame.Core;
using XFrame.Modules.Reflection;
using XFrame.Utility;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugCommandClass]
    public class CmdList
    {
        [DebugCommand]
        public static void clear_user_data()
        {
            Global.Archive.DeleteAll();
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }

        [DebugCommand]
        public static void clear()
        {
            Debugger debugger = (Debugger)Global.Debugger;
            debugger.InnerClearCmd();
        }

        [DebugCommand]
        public static void close()
        {
            Debugger debugger = (Debugger)Global.Debugger;
            debugger.InnerClose();
        }

        [DebugCommand]
        public static void collapse()
        {
            Debugger debugger = (Debugger)Global.Debugger;
            debugger.InnerCollapse(1);
        }

        [DebugCommand]
        public static void expend()
        {
            Debugger debugger = (Debugger)Global.Debugger;
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
            Debugger debugger = (Debugger)Global.Debugger;
            debugger.InnerSwitchFPS(open);
        }

        [DebugCommand]
        public static void time_scale(float timeScale)
        {
            Time.timeScale = timeScale;
        }

        [DebugCommand]
        public static void open_ui(string uiName, int resModule)
        {
            Type type = InnerGetUIType(uiName);
            if (type != null)
                Global.UI.Open(type, null, resModule);
        }

        public static void close_ui(string uiName)
        {
            Type type = InnerGetUIType(uiName);
            if (type != null)
                Global.UI.Close(type);
        }

        private static Type InnerGetUIType(string uiName)
        {
            if (string.IsNullOrEmpty(uiName))
                return null;
            TypeSystem typeSys = Global.Type.GetOrNew<IUI>();
            if (!typeSys.TryGetByName(uiName, out Type type))
            {
                foreach (Type uiType in typeSys)
                {
                    string simpleName = TypeUtility.GetSimpleName(uiType);
                    if (simpleName == uiName)
                    {
                        type = uiType;
                        break;
                    }
                }
            }
            return type;
        }
    }
}

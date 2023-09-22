using System;
using XFrame.Modules.Diagnotics;

namespace UnityXFrameLib.Diagnotics
{
    public class LogExt
    {
        public static Action Debug(params object[] content)
        {
            return () => Log.Debug(content);
        }

        public static Action Warning(params object[] content)
        {
            return () => Log.Warning(content);
        }

        public static Action Error(params object[] content)
        {
            return () => Log.Error(content);
        }

        public static Action Fatal(params object[] content)
        {
            return () => Log.Fatal(content);
        }
    }
}


using CommandLine;
using UnityXFrame.Core.Diagnotics;
using XFrame.Modules.Diagnotics;

namespace Game.Test
{
    [DebugCommandClass]
    public class DebugCommand2 : IDebugCommandLine
    {
        [Value(1)]
        public string Param1 { get; set; }

        [DebugCommand]
        public void cmd2_test1()
        {
            Log.Debug($"cmd2_test1 {Param1}");
        }

        [DebugCommand]
        public void cmd2_test2()
        {
            Log.Debug($"cmd2_test2 {Param1}");
        }
    }
}

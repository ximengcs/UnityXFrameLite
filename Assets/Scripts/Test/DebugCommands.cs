
using System.Text;
using UnityEngine;
using UnityXFrame.Core.Diagnotics;

namespace Game.Test
{
    [DebugCommandClass]
    public class DebugCommands
    {
        [DebugCommand]
        public void test()
        {
            Debug.LogWarning("test exec");
        }

        [DebugCommand]
        public void test2(CommandLine cmd)
        {
            Debug.LogWarning($"test2 exec => {cmd}");
        }

        [DebugCommand]
        public void test3(string param)
        {
            Debug.LogWarning($"test3 exec {param}");
        }

        [DebugCommand]
        public void test4(string param1, string param2)
        {
            Debug.LogWarning($"test4 exec {param1} {param2}");
        }

        [DebugCommand]
        public void test5(params string[] param)
        {
            Debug.LogWarning($"test5 exec {param.Length} {string.Join(", ", param)}");
        }

        [DebugCommand]
        public void test6(string[] param)
        {
            Debug.LogWarning($"test6 exec {param.Length} {string.Join(", ", param)}");
        }

        [DebugCommand]
        public void test7(int p1)
        {
            Debug.LogWarning($"test7 exec {string.Join(", ", p1)}");
        }
    }
}

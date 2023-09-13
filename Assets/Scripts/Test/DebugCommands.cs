using UnityEngine;
using UnityXFrameLib.Tasks;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;
using UnityXFrameLib.Pools;
using UnityXFrame.Core.Diagnotics;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;
using UnityXFrame.Core;
using System;
using Unity.VisualScripting;
using System.Linq;
using UnityXFrame.Core.Parser;
using XFrame.Modules.Diagnotics;
using XFrame.Core;
using System.Collections.Generic;

namespace Game.Test
{
    [DebugCommandClass]
    public class DebugCommands
    {
        [DebugCommand]
        public void test()
        {
            Name name = "yanying_series#1_layer#2_pos#l";
            Name name2 = "yanying_series#2_layer#2_pos#l";
            Name name3 = "yanying_series#1_layer#2_pos#l";
            Name name4 = "layer#2_pos#l_series#1_yanying";
            Debug.LogWarning(name.GetHashCode().ToString());
            Debug.LogWarning(name2.GetHashCode().ToString());
            Debug.LogWarning(name3.GetHashCode().ToString());
            Debug.LogWarning(name4.GetHashCode().ToString());

            Dictionary<Name, string> map = new Dictionary<Name, string>();
            map.Add(name, name);
            map.Add(name2, name2);
            //map.Add(name3, name3);
            //map.Add(name4, name4);

            Debug.LogWarning(map.ContainsName(name));
            Debug.LogWarning(map.ContainsName("yanying_series#1_layer#2_pos#l"));
            Debug.LogWarning(map.ContainsName("yanying_series#2_layer#2_pos#l"));
            Debug.LogWarning(map.ContainsName("yanying_series#3_layer#2_pos#l"));

            //Debug.LogWarning(name == null);
            //Debug.LogWarning(name != null);
            //Debug.LogWarning(name == "yanying");
            //Debug.LogWarning(name != "yanying");
            //Debug.LogWarning(name == name2);
            //Debug.LogWarning(name != name2);
            //Debug.LogWarning(name == name3);
            //Debug.LogWarning(name != name3);
            //Debug.LogWarning(name == name4);
            //Debug.LogWarning(name != name4);
            //Debug.LogWarning(name.Is("yanying"));
            //Debug.LogWarning(name.Is("series", 1));
            //Debug.LogWarning(name.Is("series", 2));
            //Debug.LogWarning(name.Is("layer", 2));
            //Debug.LogWarning("============");
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
        public void test7(int p1, string param)
        {
            Debug.LogWarning($"test7 exec {string.Join(", ", p1, param)}");
        }

        [DebugCommand]
        public void task()
        {
            Debug.LogWarning($"start {TimeModule.Inst.Frame}");
            TaskExt.NextFrame(() => Debug.LogWarning($"nextframe {TimeModule.Inst.Frame}"))
                .NextFrame(() => Debug.LogWarning($"nextframe2 {TimeModule.Inst.Frame}"))
                .Delay(1.0f, () => Debug.LogWarning($"delay 1s {TimeModule.Inst.Frame}"))
                .Invoke(() => Debug.LogWarning($"invoke 1 {TimeModule.Inst.Frame}"))
                .Invoke(() =>
                {
                    Debug.LogWarning($"invoke 2 {TimeModule.Inst.Frame}");
                    return UnityEngine.Random.Range(0, 3) == 0;
                })
                .Invoke(() =>
                {
                    Debug.LogWarning($"invoke 3 {TimeModule.Inst.Frame}");
                    return TaskBase.MAX_PRO;
                })
                .Beat(1.0f, () =>
                {
                    Debug.LogWarning($"beat {TimeModule.Inst.Frame}");
                    return UnityEngine.Random.Range(0, 10) == 0;
                });
        }

        [DebugCommand]
        public void pool()
        {
            PoolExt.CollectSpwanTask().Start();
        }

        [DebugCommand]
        public void color(string pattern)
        {
            bool success = ColorParser.TryParse(pattern, out Color value);
            Log.Debug($"{success} {value}");
        }

        [DebugCommand]
        public void name(string pattern, int type)
        {
            Name name = Name.Create(pattern);
            if (name.Has(type))
            {
                Log.Debug($"{name}({name.GetHashCode()}) containes {type}, value is {name.Get(type)}");
            }
            name.Release();
        }
    }
}

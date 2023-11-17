using UnityEngine;
using UnityXFrameLib.Tasks;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;
using UnityXFrameLib.Pools;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.Parser;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Core;
using XFrame.Modules.Pools;
using UnityXFrame.Core;
using XFrame.Modules.Entities;
using UnityXFrame.Core.UIElements;
using UnityXFrameLib.UIElements;
using XFrame.Modules.Conditions;
using XFrame.Modules.Reflection;
using System;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;

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

            Debug.LogWarning(map.ContainsName("yanying_series#1_layer#2_pos#l"));
            Debug.LogWarning(map.ContainsName("yanying_series#2_layer#2_pos#l"));
            Debug.LogWarning(map.ContainsName("yanying_series#3_layer#2_pos#l"));
            Debug.LogWarning(name.Get(Name.AVATAR));
            Debug.LogWarning(name.Get("layer"));
            Debug.LogWarning(name["pos"]);
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

        private TestChar m_Char;
        [DebugCommand]
        public void test2(CommandLineData cmd)
        {
            Debug.LogWarning($"test2 exec => {cmd}");
            //Global.Task.GetOrNew<EmptyTask>().Start();
            m_Char = Global.Entity.Create<TestChar>();
        }

        [DebugCommand]
        public void test21()
        {
            MultiUIGroupHelper helper = Global.UI.MainGroup.AddHelper<MultiUIGroupHelper>();
            helper.SetEffect(new AnimatorStateEffect("Open"), new AnimatorStateEffect("Close"));
            m_Char.GetCom<SceneUICom>().MainGroup.AddHelper(helper);
        }

        [DebugCommand]
        public void test22()
        {
            m_Char.GetCom<SceneUICom>().Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0, 0, 1)), Constant.LOCAL_RES_MODULE);
        }

        [DebugCommand]
        public void test23()
        {
            TaskExt.Invoke(() =>
            {
                Debug.LogWarning("Action 1");
            }).Delay(1.0f, () =>
            {
                Debug.LogWarning("Delay 1");
            }).EndFrame(() =>
            {
                Debug.LogWarning("Exec");
            }).EndFrame(() =>
            {
                Debug.LogWarning("Exec");
            }).Delay(1.0f, () =>
            {
                Debug.LogWarning("Delay 2");
            }).Invoke(() =>
            {
                Debug.LogWarning("Action 2");
            });
        }

        [DebugCommand]
        public void test3(string param)
        {
            Debug.LogWarning($"test3 exec {param}");

            ArrayParser<UniversalParser> parser = References.Require<ArrayParser<UniversalParser>>();
            parser.Split = '|';
            parser.Parse(param);
            Debug.LogWarning(parser.Get(0).IntValue);
            Debug.LogWarning(parser.Get(1).FloatValue);
            Debug.LogWarning(parser.Get(2).GetOrAddParser<Vector2Parser>().Value);
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
            Debug.LogWarning($"start {Global.Time.Frame}");
            TaskExt.NextFrame(() => Debug.LogWarning($"nextframe {Global.Time.Frame}"))
                .NextFrame(() => Debug.LogWarning($"nextframe2 {Global.Time.Frame}"))
                .Delay(1.0f, () => Debug.LogWarning($"delay 1s {Global.Time.Frame}"))
                .Invoke(() => Debug.LogWarning($"invoke 1 {Global.Time.Frame}"))
                .Invoke(() =>
                {
                    Debug.LogWarning($"invoke 2 {Global.Time.Frame}");
                    return UnityEngine.Random.Range(0, 3) == 0;
                })
                .Invoke(() =>
                {
                    Debug.LogWarning($"invoke 3 {Global.Time.Frame}");
                    return TaskBase.MAX_PRO;
                })
                .Beat(1.0f, () =>
                {
                    Debug.LogWarning($"beat {Global.Time.Frame}");
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

        private IConditionGroupHandle m_Group;

        [DebugCommand]
        public void cond1()
        {
            ConditionData data = new ConditionData("1|3");
            ConditionSetting cond = new ConditionSetting("test", data);
            cond.SetConditionHelperSetting(ConditionConst.LEVEL, new ConditionHelperSetting(1, false));
            m_Group = Global.Condition.Register(cond);
            m_Group.OnComplete((h) =>
            {
                Debug.LogWarning("complete");
            });
        }

        [DebugCommand]
        public void cond2(int level)
        {
            Global.Condition.Event.TriggerNow(ConditionEvent.Create(ConditionConst.LEVEL, level));
        }

        [DebugCommand]
        public void cond3(int level)
        {
            Global.Condition.Event.TriggerNow(ConditionGroupEvent.Create(m_Group, ConditionConst.LEVEL, level));
        }


        [DebugCommand]
        public void cond4(int level)
        {
            Global.Condition.Event.TriggerNow(ConditionGroupEvent.Create(m_Group, ConditionConst.LEVEL, level));
        }

        [DebugCommand]
        public void s1()
        {
            Global.SpriteAtlas.AddEntry(Global.Res.Load<TextAsset>("Config/atlas_map.txt").text);
            Sprite sprite = Global.Res.Load<Sprite>("Assets/Data/Textures/test2.png");
            Debug.LogWarning(sprite.name);
            GameObject inst = new GameObject();
            inst.AddComponent<SpriteRenderer>().sprite = sprite;
        }

        [DebugCommand]
        public void s2()
        {
            Global.SpriteAtlas.AddEntry(Global.Res.Load<TextAsset>("Config/atlas_map.txt").text);
            Global.Res.LoadAsync<Sprite>("Assets/Data/Textures/test2.png").OnComplete((sprite) =>
            {
                Debug.LogWarning(sprite.name);
                GameObject inst = new GameObject();
                inst.AddComponent<SpriteRenderer>().sprite = sprite;
            }).Start();
        }

        [DebugCommand]
        public void s3()
        {
            Global.SpriteAtlas.AddEntry(Global.Res.Load<TextAsset>("Config/atlas_map.txt").text);
            Sprite sprite = (Sprite)Global.Res.Load("Assets/Data/Textures/test2.png", typeof(Sprite));
            Debug.LogWarning(sprite.name);
            GameObject inst = new GameObject();
            inst.AddComponent<SpriteRenderer>().sprite = sprite;
        }

        [DebugCommand]
        public void s4()
        {
            Global.SpriteAtlas.AddEntry(Global.Res.Load<TextAsset>("Config/atlas_map.txt").text);
            Global.Res.LoadAsync("Assets/Data/Textures/test2.png", typeof(Sprite)).OnComplete((obj) =>
            {
                Sprite sprite = obj as Sprite;
                Debug.LogWarning(sprite.name);
                GameObject inst = new GameObject();
                inst.AddComponent<SpriteRenderer>().sprite = sprite;
            }).Start();
        }
    }
}

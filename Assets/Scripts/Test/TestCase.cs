using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Core.Procedure;
using System;
using System.Collections.Generic;
using System.Data;
using Test;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityXFrame.Core;
using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.HotUpdate;
using UnityXFrame.Core.Resource;
using UnityXFrame.Core.UIElements;
using UnityXFrameLib.Improve;
using UnityXFrameLib.UIElements;
using XFrame.Core;
using XFrame.Modules.Archives;
using XFrame.Modules.Datas;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Event;
using XFrame.Modules.Local;
using XFrame.Modules.Pools;
using XFrame.Modules.Resource;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

namespace Game.Test
{
    [DebugWindow()]
    public class TestCase : IDebugWindow
    {
        private float m_Time;

        public void OnAwake()
        {

            m_Time = 0.5f;
        }
        private IEventSystem m_Sys;

        private void Inner1(XEvent e)
        {
            Debug.LogWarning("Inner1");
        }

        private bool finish = false;
        private bool Inner2(XEvent e)
        {
            Debug.LogWarning("Inner2");
            return finish;
        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Test"))
            {
                ResLoadTask<TextAsset> task = XModule.Res.LoadAsync<TextAsset>("Config/Perch.txt");
                Log.Debug($"Task {task.GetHashCode()}");
                task.OnComplete((asset) =>
                {
                    Log.Debug(asset.text);
                }).StartWithDelete();
                GameObject prefab = XModule.LocalRes.Load<GameObject>("Data/Prefab/Test.prefab");
                Log.Debug(prefab.name);
            }
            if (DebugGUI.Button("Test2"))
            {
                ResLoadTask<TextAsset> task = XModule.Res.LoadAsync<TextAsset>("Config/Prop.csv");
                Log.Debug($"Task {task.GetHashCode()}");
                task.OnComplete((asset) =>
                {
                    Log.Debug(asset.text);
                }).StartWithDelete();
            }
            if (DebugGUI.Button("Download 1"))
            {
                Download(new HashSet<string>() { "Config/Perch.txt" });
            }
            if (DebugGUI.Button("Download 2"))
            {
                Download(new HashSet<string>() { "Assets/Data/Textures/Perch.txt" });
            }
            if (DebugGUI.Button("Download 3"))
            {
                Download(new HashSet<string>() { "Config/Perch.txt", "Assets/Data/Textures/Perch.txt" });
            }

            if (DebugGUI.Button("Test"))
            {
                CDTimer timer1 = CDTimer.Create("timer1");
                CDTimer timer2 = CDTimer.Create("timer2");
                CDTimer timer3 = CDTimer.Create("timer3");
                timer1.Record(10);
                timer1.Reset();
                timer2.Record(20);
                timer2.Reset();
                timer3.Record(30);
                timer3.Reset();
            }
            if (DebugGUI.Button("New EvtSys"))
            {
                m_Sys = XModule.Event.NewSys();
            }
            if (DebugGUI.Button("Test1"))
            {
                m_Sys.Listen(1, Inner1);

            }
            if (DebugGUI.Button("Test2"))
            {
                m_Sys.Listen(1, Inner2);
            }
            if (DebugGUI.Button("Test3"))
            {
                m_Sys.Trigger(1);
            }
            if (DebugGUI.Button("Test4"))
            {
                finish = true;
            }
            if (DebugGUI.Button("Add Table"))
            {
                XModule.Res.LoadAsync<TextAsset>("Config/Prop.csv").OnComplete((asset) =>
                {
                    //DataModule.Inst.Add<Prop>(asset.text, Constant.CSV_TYPE);
                    Debug.LogWarning("complete");
                }).OnUpdate((pro) =>
                {
                    Debug.LogWarning(pro);
                }).Start();
            };

            if (DebugGUI.Button("Read Table"))
            {
                Debug.LogWarning(XModule.Data.GetItem<Prop>(1).ToString());
            }

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Main");
            if (DebugGUI.Button("-"))
                XModule.UI.MainGroup.Layer--;
            if (DebugGUI.Button("+"))
                XModule.UI.MainGroup.Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test1");
            if (DebugGUI.Button("-"))
                XModule.UI.GetOrNewGroup("Test1").Layer--;
            if (DebugGUI.Button("+"))
                XModule.UI.GetOrNewGroup("Test1").Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test2");
            if (DebugGUI.Button("-"))
                XModule.UI.GetOrNewGroup("Test2").Layer--;
            if (DebugGUI.Button("+"))
                XModule.UI.GetOrNewGroup("Test2").Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test1 UI");
            if (DebugGUI.Button("-"))
                XModule.UI.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0, 0, 1)), Constant.LOCAL_RES_MODULE).Layer--;
            if (DebugGUI.Button("+"))
                XModule.UI.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0, 0, 1)), Constant.LOCAL_RES_MODULE).Layer++;
            if (DebugGUI.Button("Close"))
                XModule.UI.Close<DialogUI>();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test2 UI");
            if (DebugGUI.Button("-"))
                XModule.UI.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0.2f, 0, 1)), Constant.LOCAL_RES_MODULE, 2).Layer--;
            if (DebugGUI.Button("+"))
                XModule.UI.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0.2f, 0, 1)), Constant.LOCAL_RES_MODULE, 2).Layer++;
            if (DebugGUI.Button("Close"))
                XModule.UI.Close<DialogUI>(2);
            GUILayout.EndHorizontal();

            DebugGUI.Label("Test UI");
            m_Time = DebugGUI.FloatField(m_Time);
            if (DebugGUI.Button("Test Init GameObject"))
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                GameObject prefab = XModule.LocalRes.Load<GameObject>("Data/Prefab/Test.prefab");
                GameObject.Instantiate(prefab);
                sw.Stop();
                Log.Debug("Debugger", sw.ElapsedMilliseconds);
            }

            if (DebugGUI.Button("Init Group1"))
            {
                XModule.UI.GetOrNewGroup("Test");
                OnlyOneUIGroupHelper helper = XModule.UI.MainGroup.AddHelper<OnlyOneUIGroupHelper>();
                helper.SetEffect(new FadeEffect(1, 0.5f), new MoveEffect(MoveEffect.Direct.FromLeft, false, true));
            }
            if (DebugGUI.Button("Init Group2"))
            {
                MultiUIGroupHelper helper = XModule.UI.MainGroup.AddHelper<MultiUIGroupHelper>();
                //helper.SetEffect(
                //        new AnimatorTriggerEffect("Open", "Open"),
                //        new AnimatorTriggerEffect("Close", "Close"));
                helper.SetEffect(new AnimatorStateEffect("Open"), new AnimatorStateEffect("Close"));
            }
            if (DebugGUI.Button("Init Group3"))
            {
                XModule.UI.GetOrNewGroup("Test");
                OnlyOneUIGroupHelper helper = XModule.UI.MainGroup.AddHelper<OnlyOneUIGroupHelper>();
                helper.SetEffect(new ScaleEffect(Vector2.one, 2), new ScaleEffect(Vector2.one, Vector2.zero, 2));
            }
            if (DebugGUI.Button("Init Group4"))
            {
                //UIModule.Inst.MainGroup.AddHelper<OnlyOneUIGroupHelper>((helper) =>
                //{
                //    helper.SetEffect(new FadeEffect(1, 0.5f), new FadeEffect(1, 0, 0.5f));
                //});
            }
            if (DebugGUI.Button("Open Setting"))
            {
                XModule.UI.Open<SettingUI>("Test", null, Constant.LOCAL_RES_MODULE);
            }
            if (DebugGUI.Button("Open Setting2"))
            {
                XModule.UI.Open<SettingUI>(null, Constant.LOCAL_RES_MODULE);
            }
            if (DebugGUI.Button("Open Dialog 1"))
            {
                XModule.UI.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0.2f, 0, 0, 1));
                }, Constant.LOCAL_RES_MODULE, 1);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Open Dialog 2"))
            {
                XModule.UI.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0, 0.2f, 0, 1));
                }, Constant.LOCAL_RES_MODULE, 2);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Open Dialog 3"))
            {
                XModule.UI.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0, 0.5f, 0, 1));
                }, Constant.LOCAL_RES_MODULE, 3);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Open Dialog 4"))
            {
                XModule.UI.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0, 0.8f, 0, 1));
                }, Constant.LOCAL_RES_MODULE, 4);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Close Dialog 1"))
            {
                XModule.UI.Close<DialogUI>(1);
            }
            if (DebugGUI.Button("Close Dialog 2"))
            {
                XModule.UI.Close<DialogUI>(2);
            }
            if (DebugGUI.Button("Close Dialog 3"))
            {
                XModule.UI.Close<DialogUI>(3);
            }
            if (DebugGUI.Button("Close Dialog 4"))
            {
                XModule.UI.Close<DialogUI>(4);
            }
            if (DebugGUI.Button("GC"))
            {
                //GCModule.Inst.Request()
                //    .OnComplete(() => Log.Debug("Complete GC"))
                //    .Start();
            }
            if (DebugGUI.Button("Test"))
            {
                XModule.Audio.PlayAsync("TestAudio.mp3", "Bgm")
                    .OnComplete((audio) => m_TestAudio = audio);
            }
            if (DebugGUI.Button("Test2"))
            {
                XModule.Audio.PlayLoopAsync("TestAudio.mp3", "Bgm")
                    .OnComplete((audio) => m_TestAudio = audio);
            }
            if (DebugGUI.Button("Test2"))
            {
                XModule.Audio.PlayAsync("TestAudio.mp3", "Bgm");
            }
            if (DebugGUI.Button("Stop Bgm Group"))
            {
                XModule.Audio.GetOrNewGroup("Bgm").Stop();
            }
            if (DebugGUI.Button("Stop"))
            {
                m_TestAudio.Stop();
            }
            if (DebugGUI.Button("Play"))
            {
                m_TestAudio.Play();
            }
            if (DebugGUI.Button("Pause"))
            {
                m_TestAudio.Pause();
            }
            if (DebugGUI.Button("Auto UI Task"))
            {
                UIModuleExt.CollectAutoTask().Start();
            }

            if (DebugGUI.Button("Set cmd info"))
            {
                XModule.Debugger.SetCmdHelpInfo("" +
                    "close\n" +
                    "open_ui");
            }

            m_Slider = GUILayout.HorizontalSlider(m_Slider, 1, 120, DebugGUI.Style.HorizontalSlider, DebugGUI.Style.HorizontalSliderThumb);
        }

        private float m_Slider = 80;
        private IAudio m_TestAudio;

        int pro;
        TweenerCore<int, int, NoOptions> tweener;

        public void Dispose()
        {

        }

        public void Download(HashSet<string> perchs)
        {
            Log.Debug("Start hot update check task.");
            HotUpdateCheckTask checkTask = XModule.Task.GetOrNew<HotUpdateCheckTask>();
            checkTask.OnComplete(() =>
            {
                if (checkTask.Success)
                    Log.Debug($"Hot update check task has success.");
                else
                    Log.Debug("Hot update check task has failure.");
                Log.Debug("Start hot update download task.");
                HotUpdateDownTask downTask = XModule.Task.GetOrNew<HotUpdateDownTask>();
                downTask.AddList(checkTask.ResList, perchs).OnComplete(() =>
                {
                    if (downTask.Success)
                        Log.Debug("Hot update download task has success.");
                    else
                        Log.Debug("Hot update download task has failure.");
                }).Start();
            }).Start();
        }
    }
}

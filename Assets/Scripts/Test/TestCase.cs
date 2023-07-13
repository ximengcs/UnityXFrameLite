using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
using UnityXFrame.Core.Resource;
using UnityXFrame.Core.UIs;
using UnityXFrameLib.Improve;
using UnityXFrameLib.UI;
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
                CDTimer timer1 = TimeModule.Inst.NewTimer("timer1");
                CDTimer timer2 = TimeModule.Inst.NewTimer("timer2");
                CDTimer timer3 = TimeModule.Inst.NewTimer("timer3");
                timer1.Record(10);
                timer1.Reset();
                timer2.Record(20);
                timer2.Reset();
                timer3.Record(30);
                timer3.Reset();
            }
            if (DebugGUI.Button("New EvtSys"))
            {
                m_Sys = EventModule.Inst.NewSys();
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
                ResModule.Inst.LoadAsync<TextAsset>("Assets/Data/Config/Prop.csv").OnComplete((asset) =>
                {
                    DataModule.Inst.Add<Prop>(asset.text);
                }).Start();
            };

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Main");
            if (DebugGUI.Button("-"))
                UIModule.Inst.MainGroup.Layer--;
            if (DebugGUI.Button("+"))
                UIModule.Inst.MainGroup.Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test1");
            if (DebugGUI.Button("-"))
                UIModule.Inst.GetOrNewGroup("Test1").Layer--;
            if (DebugGUI.Button("+"))
                UIModule.Inst.GetOrNewGroup("Test1").Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test2");
            if (DebugGUI.Button("-"))
                UIModule.Inst.GetOrNewGroup("Test2").Layer--;
            if (DebugGUI.Button("+"))
                UIModule.Inst.GetOrNewGroup("Test2").Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test1 UI");
            if (DebugGUI.Button("-"))
                UIModule.Inst.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0, 0, 1)), true).Layer--;
            if (DebugGUI.Button("+"))
                UIModule.Inst.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0, 0, 1)), true).Layer++;
            if (DebugGUI.Button("Close"))
                UIModule.Inst.Close<DialogUI>();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test2 UI");
            if (DebugGUI.Button("-"))
                UIModule.Inst.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0.2f, 0, 1)), true, 2).Layer--;
            if (DebugGUI.Button("+"))
                UIModule.Inst.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0.2f, 0, 1)), true, 2).Layer++;
            if (DebugGUI.Button("Close"))
                UIModule.Inst.Close<DialogUI>(2);
            GUILayout.EndHorizontal();

            DebugGUI.Label("Test UI");
            m_Time = DebugGUI.FloatField(m_Time);
            if (DebugGUI.Button("Test Init GameObject"))
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                GameObject prefab = NativeResModule.Inst.Load<GameObject>("Data/Prefab/Test.prefab");
                GameObject.Instantiate(prefab);
                sw.Stop();
                Log.Debug("Debugger", sw.ElapsedMilliseconds);
            }

            if (DebugGUI.Button("Init Group1"))
            {
                UIModule.Inst.GetOrNewGroup("Test");
                OnlyOneUIGroupHelper helper = UIModule.Inst.MainGroup.AddHelper<OnlyOneUIGroupHelper>();
                helper.SetEffect(new FadeEffect(1, 0.5f), new MoveEffect(MoveEffect.Direct.FromLeft, false, true));
            }
            if (DebugGUI.Button("Init Group2"))
            {
                //UIModule.Inst.MainGroup.AddHelper<OnlyOneUIGroupHelper>((helper) =>
                //{
                //    helper.SetEffect(
                //        new MoveEffect(MoveEffect.Direct.Rand, true, false, m_Time),
                //        new MoveEffect(MoveEffect.Direct.Rand, false, true, m_Time));
                //});
            }
            if (DebugGUI.Button("Init Group3"))
            {
                UIModule.Inst.GetOrNewGroup("Test");
                OnlyOneUIGroupHelper helper = UIModule.Inst.MainGroup.AddHelper<OnlyOneUIGroupHelper>();
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
                UIModule.Inst.Open<SettingUI>("Test", null, true);
            }
            if (DebugGUI.Button("Open Dialog 1"))
            {
                UIModule.Inst.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0.2f, 0, 0, 1));
                }, true, 1);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Open Dialog 2"))
            {
                UIModule.Inst.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0, 0.2f, 0, 1));
                }, true, 2);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Close Dialog 1"))
            {
                UIModule.Inst.Close<DialogUI>(1);
                //AudioModule.Inst.PlayAsync("a2.wav");
            }
            if (DebugGUI.Button("Close Dialog 2"))
            {
                UIModule.Inst.Close<DialogUI>(2);
                //AudioModule.Inst.PlayAsync("a2.wav");
            }
            if (DebugGUI.Button("GC"))
            {
                GCModule.Inst.Request()
                    .OnComplete(() => Log.Debug("Complete GC"))
                    .Start();
            }
            if (DebugGUI.Button("Test"))
            {
                AudioModule.Inst.PlayAsync("TestAudio.mp3", "Bgm")
                    .OnComplete((audio) => m_TestAudio = audio);
            }
            if (DebugGUI.Button("Test2"))
            {
                AudioModule.Inst.PlayLoopAsync("TestAudio.mp3", "Bgm")
                    .OnComplete((audio) => m_TestAudio = audio);
            }
            if (DebugGUI.Button("Test2"))
            {
                AudioModule.Inst.PlayAsync("TestAudio.mp3", "Bgm");
            }
            if (DebugGUI.Button("Stop Bgm Group"))
            {
                AudioModule.Inst.GetOrNewGroup("Bgm").Stop();
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
        }

        private IAudio m_TestAudio;

        int pro;
        TweenerCore<int, int, NoOptions> tweener;

        public void Dispose()
        {

        }
    }
}

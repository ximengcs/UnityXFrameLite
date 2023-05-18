using System;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.Resource;
using UnityXFrame.Core.UIs;
using UnityXFrameLib.Improve;
using UnityXFrameLib.UI;
using XFrame.Core;
using XFrame.Modules.Archives;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Local;
using XFrame.Modules.Resource;

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

        public void OnDraw()
        {
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
                //UIModule.Inst.MainGroup.AddHelper<OnlyOneUIGroupHelper>((helper) =>
                //{
                //    helper.SetEffect(new FadeEffect(1, 0.5f), new MoveEffect(MoveEffect.Direct.FromLeft, false, true));
                //});
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
                //UIModule.Inst.MainGroup.AddHelper<OnlyOneUIGroupHelper>((helper) =>
                //{
                //    helper.SetEffect(new ScaleEffect(Vector2.one), new ScaleEffect(Vector2.one, Vector2.zero));
                //});
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
                AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Close Dialog 1"))
            {
                UIModule.Inst.Close<DialogUI>(1);
                AudioModule.Inst.PlayAsync("a2.wav");
            }
            if (DebugGUI.Button("Close Dialog 2"))
            {
                UIModule.Inst.Close<DialogUI>(2);
                AudioModule.Inst.PlayAsync("a2.wav");
            }
            if (DebugGUI.Button("GC"))
            {
                GCModule.Inst.Request()
                    .OnComplete(() => Log.Debug("Complete GC"))
                    .Start();
            }
            if (DebugGUI.Button("Test"))
            {
                CsvArchive csv = ArchiveModule.Inst.GetOrNew<CsvArchive>("test_archive");
                if (csv.Data.Row > 0)
                {
                    Debug.LogWarning(csv.Data.Get(0)[0]);
                }
                else
                {
                    Debug.LogWarning("col" + csv.Data.Column);
                    csv.Data.Add()[0] = "1";
                }
            }
        }

        public void Dispose()
        {

        }
    }
}

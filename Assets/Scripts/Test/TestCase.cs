using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Core.Procedure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;
using UnityEngine.Scripting;
using UnityEngine.UIElements;
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
using XFrame.Modules.NewTasks;
using XFrame.Modules.Pools;
using XFrame.Modules.Resource;
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

        private XTask m_Task;
        private XTask m_Task2;

        private async XTask InnerTest()
        {
            Debug.LogWarning("1");
            await Task.Delay(1000);
            Debug.LogWarning("2");
            await Task.Delay(3000);
            Debug.LogWarning("3");
            m_Task2 = InnerTest2();
            m_Task2.OnCompleted(() => { Log.Debug("Complete task2"); });
            await m_Task2;
        }

        private bool TheData;

        public async Task<bool> GetTheData()
        {
            while (TheData != true)
                await Task.Yield();

            return TheData;
        }

        private async XTask InnerTest2()
        {
            Debug.LogWarning("4");
            await Task.Delay(3000);
            Debug.LogWarning("5");
            int value = await InnerTest3();
            Debug.LogWarning($"value {value}");
            InnerTest4();
        }

        private async XTask<int> InnerTest3()
        {
            await Task.Delay(1000);
            return 9;
        }

        private async XVoid InnerTest4()
        {
            Debug.LogWarning("InnerTest4 1");
            await Task.Delay(1000);
            InnerTest5();
            Debug.LogWarning("InnerTest4 2");
        }

        private async XVoid InnerTest5()
        {
            Debug.LogWarning("InnerTest4 1");
            Debug.LogWarning("InnerTest4 2");
        }

        private XProTask protask;

        private async XTask InnerTest6()
        {
            Debug.LogWarning("Start");
            protask = new XProTask(new DOTweenProgress());
            protask.OnUpdate((pro) => Debug.LogWarning($"Pro {pro}"));
            protask.OnComplete(() => Debug.LogWarning("OnComplete"));
            await protask;
            Debug.LogWarning("End");
        }

        private async XTask InnerTestPost()
        {
            WWWForm form = new WWWForm();
            form.AddField("Id", 1);
            var op = Addressables.LoadAssetAsync<Texture2D>("Data2/Textures/QQQ/test2.png");
            op.WaitForCompletion();

            var data = op.Result.GetRawTextureData();
            
            Debug.LogWarning(data.Length);
            form.AddBinaryData("portrait", data, "screenShot.png", "image/png");
            Debug.LogWarning(op.Result.width + " " + op.Result.height);
            UnityWebRequest req = UnityWebRequest.Post("http://10.104.15.229:3000/users/update-basic", form);
            req.SetRequestHeader("UserId", "1");
            await GetAwaiter(req.SendWebRequest());
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                //Debug.Log(req.downloadHandler.text);
            }
        }

        private byte[] data;
        private async XTask InnerTestGet()
        {
            UnityWebRequest req = UnityWebRequest.Get("http://localhost:3000/users/basic?Id=1");
            req.SetRequestHeader("UserId", "1");
            await GetAwaiter(req.SendWebRequest());
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.LogWarning(req.downloadHandler.text);
            }
            req = UnityWebRequest.Get($"http://localhost:3000/users/portrait?Id=1");
            req.SetRequestHeader("UserId", "1");
            await GetAwaiter(req.SendWebRequest());
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                data = req.downloadHandler.data;
                Debug.LogWarning(data == null);
            }
        }

        public static Task GetAwaiter(AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += obj => { tcs.SetResult(null); };
            return tcs.Task;
        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Test"))
            {
                InnerTest().Coroutine();
            }

            if (DebugGUI.Button("Cancel"))
            {
                m_Task.Cancel(true);
            }

            if (DebugGUI.Button("Cancel2"))
            {
                Texture2D tex = new Texture2D(320, 320, TextureFormat.ASTC_6x6, 1, true);
                Debug.LogWarning(data.Length);
                tex.LoadRawTextureData(data);
                tex.Apply();
                GameObject obj = new GameObject();
                obj.AddComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }

            if (DebugGUI.Button("Test2"))
            {
                InnerTest6().Coroutine();
            }

            if (DebugGUI.Button("Cancel Test2"))
            {
                protask.Cancel();
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
                m_Sys = Global.Event.NewSys();
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
                Global.Res.LoadAsync<TextAsset>("Config/Prop.csv").OnComplete((asset) =>
                {
                    //DataModule.Inst.Add<Prop>(asset.text, Constant.CSV_TYPE);
                    Debug.LogWarning("complete");
                }).OnUpdate((pro) => { Debug.LogWarning(pro); }).Start();
            }

            ;
            if (DebugGUI.Button("Table"))
            {
                TextAsset text = Global.Res.Load<TextAsset>("Config/Prop.csv");
                Debug.LogWarning(text.text);
            }

            if (DebugGUI.Button("Read Table"))
            {
                Debug.LogWarning(Global.Data.GetItem<Prop>(1).ToString());
            }

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Main");
            if (DebugGUI.Button("-"))
                Global.UI.MainGroup.Layer--;
            if (DebugGUI.Button("+"))
                Global.UI.MainGroup.Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test1");
            if (DebugGUI.Button("-"))
                Global.UI.GetOrNewGroup("Test1").Layer--;
            if (DebugGUI.Button("+"))
                Global.UI.GetOrNewGroup("Test1").Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test2");
            if (DebugGUI.Button("-"))
                Global.UI.GetOrNewGroup("Test2").Layer--;
            if (DebugGUI.Button("+"))
                Global.UI.GetOrNewGroup("Test2").Layer++;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test1 UI");
            if (DebugGUI.Button("-"))
                Global.UI.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0, 0, 1)), Constant.LOCAL_RES_MODULE)
                    .Layer--;
            if (DebugGUI.Button("+"))
                Global.UI.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0, 0, 1)), Constant.LOCAL_RES_MODULE)
                    .Layer++;
            if (DebugGUI.Button("Close"))
                Global.UI.Close<DialogUI>();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Test2 UI");
            if (DebugGUI.Button("-"))
                Global.UI.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0.2f, 0, 1)), Constant.LOCAL_RES_MODULE, 2)
                    .Layer--;
            if (DebugGUI.Button("+"))
                Global.UI.Open<DialogUI>((ui) => ui.SetData(new Color(0.2f, 0.2f, 0, 1)), Constant.LOCAL_RES_MODULE, 2)
                    .Layer++;
            if (DebugGUI.Button("Close"))
                Global.UI.Close<DialogUI>(2);
            GUILayout.EndHorizontal();

            DebugGUI.Label("Test UI");
            m_Time = DebugGUI.FloatField(m_Time);
            if (DebugGUI.Button("Test Init GameObject"))
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                GameObject prefab = Global.LocalRes.Load<GameObject>("Data/Prefab/Test.prefab");
                GameObject.Instantiate(prefab);
                sw.Stop();
                Log.Debug("Debugger", sw.ElapsedMilliseconds);
            }

            if (DebugGUI.Button("Init Group1"))
            {
                Global.UI.GetOrNewGroup("Test");
                OnlyOneUIGroupHelper helper = Global.UI.MainGroup.AddHelper<OnlyOneUIGroupHelper>();
                helper.SetEffect(new FadeEffect(1, 0.5f), new MoveEffect(MoveEffect.Direct.FromLeft, false, true));
            }

            if (DebugGUI.Button("Init Group2"))
            {
                MultiUIGroupHelper helper = Global.UI.MainGroup.AddHelper<MultiUIGroupHelper>();
                //helper.SetEffect(
                //        new AnimatorTriggerEffect("Open", "Open"),
                //        new AnimatorTriggerEffect("Close", "Close"));
                helper.SetEffect(new AnimatorStateEffect("Open"), new AnimatorStateEffect("Close"));
            }

            if (DebugGUI.Button("Init Group3"))
            {
                Global.UI.GetOrNewGroup("Test");
                OnlyOneUIGroupHelper helper = Global.UI.MainGroup.AddHelper<OnlyOneUIGroupHelper>();
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
                Global.UI.Open<SettingUI>("Test", null, Constant.LOCAL_RES_MODULE);
            }

            if (DebugGUI.Button("Open Setting2"))
            {
                Global.UI.Open<SettingUI>(null, Constant.LOCAL_RES_MODULE);
            }

            if (DebugGUI.Button("Open Dialog 1"))
            {
                Global.UI.Open<DialogUI>((ui) => { ui.SetData(new Color(0.2f, 0, 0, 1)); }, Constant.LOCAL_RES_MODULE,
                    1);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }

            if (DebugGUI.Button("Open Dialog 2"))
            {
                Global.UI.Open<DialogUI>((ui) => { ui.SetData(new Color(0, 0.2f, 0, 1)); }, Constant.LOCAL_RES_MODULE,
                    2);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }

            if (DebugGUI.Button("Open Dialog 3"))
            {
                Global.UI.Open<DialogUI>((ui) => { ui.SetData(new Color(0, 0.5f, 0, 1)); }, Constant.LOCAL_RES_MODULE,
                    3);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }

            if (DebugGUI.Button("Open Dialog 4"))
            {
                Global.UI.Open<DialogUI>((ui) => { ui.SetData(new Color(0, 0.8f, 0, 1)); }, Constant.COMMON_RES_MODULE,
                    4);
                //AudioModule.Inst.PlayAsync("a1.wav");
            }

            if (DebugGUI.Button("Close Dialog 1"))
            {
                Global.UI.Close<DialogUI>(1);
            }

            if (DebugGUI.Button("Close Dialog 2"))
            {
                Global.UI.Close<DialogUI>(2);
            }

            if (DebugGUI.Button("Close Dialog 3"))
            {
                Global.UI.Close<DialogUI>(3);
            }

            if (DebugGUI.Button("Close Dialog 4"))
            {
                Global.UI.Close<DialogUI>(4);
            }

            if (DebugGUI.Button("GC"))
            {
                //GCModule.Inst.Request()
                //    .OnComplete(() => Log.Debug("Complete GC"))
                //    .Start();
            }

            if (DebugGUI.Button("Test"))
            {
                Global.Audio.PlayAsync("TestAudio.mp3", "Bgm")
                    .OnComplete((audio) => m_TestAudio = audio);
            }

            if (DebugGUI.Button("Test2"))
            {
                Global.Audio.PlayLoopAsync("TestAudio.mp3", "Bgm")
                    .OnComplete((audio) => m_TestAudio = audio);
            }

            if (DebugGUI.Button("Test2"))
            {
                Global.Audio.PlayAsync("TestAudio.mp3", "Bgm");
            }

            if (DebugGUI.Button("Stop Bgm Group"))
            {
                Global.Audio.GetOrNewGroup("Bgm").Stop();
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
                Global.Debugger.SetCmdHelpInfo("" +
                                               "close\n" +
                                               "open_ui");
            }

            m_Slider = GUILayout.HorizontalSlider(m_Slider, 1, 120, DebugGUI.Style.HorizontalSlider,
                DebugGUI.Style.HorizontalSliderThumb);
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
            HotUpdateCheckTask checkTask = Global.Task.GetOrNew<HotUpdateCheckTask>();
            checkTask.OnComplete(() =>
            {
                if (checkTask.Success)
                    Log.Debug($"Hot update check task has success.");
                else
                    Log.Debug("Hot update check task has failure.");
                Log.Debug("Start hot update download task.");
                HotUpdateDownTask downTask = Global.Task.GetOrNew<HotUpdateDownTask>();
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

    public class DOTweenProgress : IProTaskHandler
    {
        private float m_Pro;

        public object Data { get; }
        public bool IsDone => m_Pro >= 1;
        public float Pro => m_Pro;

        public DOTweenProgress()
        {
            m_Pro = 0;
            DOTween.To(
                () => m_Pro,
                (v) => m_Pro = v, 1, 5);
        }
    }
}
using System;
using Game.Test;
using UnityEngine;
using UnityXFrame.Core;
using UnityXFrame.Core.UIs;
using XFrame.Modules.Tasks;
using UnityXFrame.Core.Audios;
using XFrame.Modules.Resource;
using UnityXFrame.Core.Diagnotics;
using XFrame.Modules.Containers;

namespace XHotfix.Test
{
    [DebugHelp("框架测试窗口")]
    public class TestFrame : IDebugWindow
    {
        private int m_Group;
        private int m_UI = 1;
        private int m_Layer;
        private int m_GroupLayer;

        private IAudio m_Audio1;
        private IAudio m_Audio2;

        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            DebugGUI.Label("1");
            if (DebugGUI.Button("Create entity"))
            {
                ResModule.Inst.LoadAsync("Textures/iloveu.png", typeof(Sprite))
                    .OnComplete((asset) =>
                    {
                        if (asset != null)
                        {
                            Sprite sprite = (Sprite)asset;
                            GameObject inst = new GameObject();
                            inst.name = sprite.name;
                            inst.AddComponent<SpriteRenderer>().sprite = sprite;
                        }
                    }).Start();
            }

            m_UI = DebugGUI.IntField(m_UI);
            m_Group = DebugGUI.IntField(m_Group);
            m_Layer = DebugGUI.IntField(m_Layer);
            m_GroupLayer = DebugGUI.IntField(m_GroupLayer);
            if (DebugGUI.Button("Open UI"))
                UIModule.Inst.Open("Game.Test.LoadingUI", (data) => data.SetData(0.5f), true);
            if (DebugGUI.Button("Close UI"))
                UIModule.Inst.Close<LoadingUI>();
            if (DebugGUI.Button("Destroy UI"))
                UIModule.Inst.DestroyUI<LoadingUI>();

            if (DebugGUI.Button("Set Group"))
            {
                IUIGroup testGroup = UIModule.Inst.GetOrNewGroup("TestGroup");
                IUIGroup test2Group = UIModule.Inst.GetOrNewGroup("TestOnlyOneGroup");
                IUIGroup mainGroup = UIModule.Inst.MainGroup;
                mainGroup.Layer = 0;
                testGroup.Layer = 1;
                test2Group.Layer = 1;
                testGroup.AddHelper<TestUIGroupHelper>();
                test2Group.AddHelper<TestOnlyOneUIGroupHelper>();
                mainGroup.AddHelper<TestMainGroupHelper>();
            }
            if (DebugGUI.Button("Open Dialog"))
                UIModule.Inst.Open<DialogUI>("TestGroup", (data) =>
                {
                    data.SetData(new Vector2(100, 100));
                    data.SetData(new Color(0.5f, 0, 0, 1));
                }, true, 1);
            if (DebugGUI.Button("Open Dialog2"))
                UIModule.Inst.Open<DialogUI>("TestGroup", (data) =>
                {
                    data.SetData(new Vector2(-100, -100));
                    data.SetData(new Color(0, 0.5f, 0, 1));
                }, true, 2);
            if (DebugGUI.Button("Open Dialog3"))
                UIModule.Inst.Open<DialogUI>("TestOnlyOneGroup", (data) =>
                {
                    data.SetData(new Vector2(50, 50));
                    data.SetData(new Color(0.5f, 0.5f, 0, 1));
                }, true, 3);
            if (DebugGUI.Button("Open Dialog4"))
                UIModule.Inst.Open<DialogUI>("TestOnlyOneGroup", (data) =>
                {
                    data.SetData(new Vector2(-50, -50));
                    data.SetData(new Color(0.5f, 0, 0.5f, 1));
                }, true, 4);
            if (DebugGUI.Button("Close Dialog"))
                UIModule.Inst.Close<DialogUI>(1);
            if (DebugGUI.Button("Close Dialog2"))
                UIModule.Inst.Close<DialogUI>(2);
            if (DebugGUI.Button("Close Dialog3"))
                UIModule.Inst.Close<DialogUI>(3);
            if (DebugGUI.Button("Close Dialog4"))
                UIModule.Inst.Close<DialogUI>(4);

            if (DebugGUI.Button("Open UI To Group"))
                UIModule.Inst.Open($"TestUI{m_UI}", $"Group{m_UI}", default, true);
            if (DebugGUI.Button("Set Layer"))
                UIModule.Inst.Get($"TestUI{m_UI}").Layer = m_Layer;
            if (DebugGUI.Button("Set Group Layer"))
                UIModule.Inst.MainGroup.Layer = m_GroupLayer;

            if (DebugGUI.Button("Test"))
            {
                Debug.LogWarning(Faker.Name.FullName());
            }
            if (DebugGUI.Button("Test Group"))
            {
                AudioModule.Inst.GetOrNewGroup("AudioEffect").Volume = 1;
                AudioModule.Inst.MainGroup.Volume = 0.2f;
            }
            if (DebugGUI.Button("Play bgm1"))
            {
                AudioModule.Inst.PlayLoopAsync("TestAudio.mp3")
                    .OnComplete((audio) =>
                    {
                        m_Audio1?.Stop();
                        m_Audio1 = audio;
                    });
            }
            if (DebugGUI.Button("Play1 bgm2"))
            {
                AudioModule.Inst.PlayLoopAsync("TestAudio2.mp3")
                    .OnComplete((audio) =>
                    {
                        m_Audio2?.Stop();
                        m_Audio2 = audio;
                    });
            }
            if (DebugGUI.Button("Stop bgm1"))
            {
                m_Audio1?.Stop();
                m_Audio1 = null;
            }
            if (DebugGUI.Button("Stop bgm2"))
            {
                m_Audio2?.Stop();
                m_Audio2 = null;
            }

            if (DebugGUI.Button("Test Preload"))
            {
                ResModule.Inst.Preload(
                    new string[] { $"{Constant.AUDIO_PATH}/TestAudio.mp3" },
                    new Type[] { typeof(AudioClip) })
                    .Start();
            }
            if (DebugGUI.Button("Play BGM1"))
                AudioModule.Inst.PlayLoop("TestAudio.mp3");

            if (DebugGUI.Button("Play1"))
                AudioModule.Inst.PlayAsync("a1.wav", "AudioEffect");
            if (DebugGUI.Button("Play2"))
                AudioModule.Inst.PlayAsync("a2.wav", "AudioEffect");
            if (DebugGUI.Button("Play3"))
                AudioModule.Inst.PlayAsync("a3.wav", "AudioEffect");
        }
    }
}

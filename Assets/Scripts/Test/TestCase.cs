using UnityEngine;
using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.UIs;
using UnityXFrameLib.UI;

namespace Game.Test
{
    [DebugWindow()]
    public class TestCase : IDebugWindow
    {
        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Init Group"))
            {
                UIModule.Inst.MainGroup.AddHelper<MultiUIGroupHelper>((helper) =>
                {
                    //helper.SetEffect(new FadeEffect(1, 0.5f), new MoveEffect(MoveEffect.Direct.FromLeft, false, true));
                    helper.SetEffect(
                        new MoveEffect(MoveEffect.Direct.Rand, true, false), 
                        new MoveEffect(MoveEffect.Direct.Rand, false, true));
                    //helper.SetEffect(new ScaleEffect(Vector2.one), new ScaleEffect(Vector2.one, Vector2.zero));
                    //helper.SetEffect(new FadeEffect(1, 0.5f), new FadeEffect(1, 0, 0.5f));
                });
            }
            if (DebugGUI.Button("Open Dialog 1"))
            {
                UIModule.Inst.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0.2f, 0, 0, 1));
                }, true, 1);
                AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Close Dialog 1"))
            {
                UIModule.Inst.Close<DialogUI>(1);
                AudioModule.Inst.PlayAsync("a2.wav");
            }
            if (DebugGUI.Button("Open Dialog 2"))
            {
                UIModule.Inst.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0, 0.2f, 0, 1));
                }, true, 2);
                AudioModule.Inst.PlayAsync("a1.wav");
            }
            if (DebugGUI.Button("Close Dialog 2"))
            {
                UIModule.Inst.Close<DialogUI>(2);
                AudioModule.Inst.PlayAsync("a2.wav");
            }
        }

        public void Dispose()
        {

        }
    }
}

using UnityEngine;
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
                    helper.SetEffect(new FadeEffect(1, 0.5f), new MoveEffect(MoveEffect.Direct.FromLeft, false));
                });
            }
            if (DebugGUI.Button("Open Dialog 1"))
            {
                UIModule.Inst.Open<DialogUI>((ui) =>
                {
                    ui.SetData(new Color(0.2f, 0, 0, 1));
                }, true);
            }
            if (DebugGUI.Button("Close Dialog 1"))
            {
                UIModule.Inst.Close<DialogUI>();
            }
        }

        public void Dispose()
        {

        }
    }
}

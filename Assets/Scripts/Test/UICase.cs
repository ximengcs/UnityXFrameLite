
using Test;
using UnityEngine;
using UnityXFrame.Core;
using UnityXFrame.Core.Diagnotics;
using UnityXFrameLib.UIElements;

namespace Game.Test
{
    [DebugWindow(1000)]
    public class UICase : IDebugWindow
    {
        public void OnDraw()
        {
            //if (DebugGUI.Button("Init Fade Group"))
            //{
            //    OnlyOneUIGroupHelper helper = Global.UI.MainGroup.AddHelper<OnlyOneUIGroupHelper>();
            //    helper.SetEffect(new FadeEffect(1, 0.5f), new MoveEffect(MoveEffect.Direct.FromLeft, false, true));
            //}

            if (DebugGUI.Button("Init Anim Trigger Group"))
            {
                MultiUIGroupHelper helper = Global.UI.MainGroup.AddHelper<MultiUIGroupHelper>();
                helper.SetEffect(
                        new AnimatorTriggerEffect("Open", "Open"),
                        new AnimatorTriggerEffect("Close", "Close"));
                helper.SetEffect(new AnimatorStateEffect("Open"), new AnimatorStateEffect("Close"));
            }

            //if (DebugGUI.Button("Init Anim State Group"))
            //{
            //    MultiUIGroupHelper helper = Global.UI.MainGroup.AddHelper<MultiUIGroupHelper>();
            //    helper.SetEffect(new AnimatorStateEffect("Open"), new AnimatorStateEffect("Close"));
            //}
            //
            //if (DebugGUI.Button("Init Scale Group"))
            //{
            //    OnlyOneUIGroupHelper helper = Global.UI.MainGroup.AddHelper<OnlyOneUIGroupHelper>();
            //    helper.SetEffect(new ScaleEffect(Vector2.one, 2), new ScaleEffect(Vector2.one, Vector2.zero, 2));
            //}

            if (DebugGUI.Button("Open Layout"))
            {
                Global.UI.Open<LayoutUI>(null, Constant.LOCAL_RES_MODULE);
            }

            if (DebugGUI.Button("Open LayoutLeft UI"))
            {
                Global.UI.Get<LayoutUI>().Left.Open<BackgroundUI>(null, Constant.LOCAL_RES_MODULE);
            }

            if (DebugGUI.Button("Open LayoutRight UI"))
            {
                Global.UI.Get<LayoutUI>().Right.Open<DialogUI>(
                    (ui) => ui.SetData(new Color(0, 0.2f, 0, 1)), Constant.LOCAL_RES_MODULE);
            }

            if (DebugGUI.Button("Open Left UI"))
            {
                Global.UI.Get<LayoutUI>().OpenLeft();
            }

            if (DebugGUI.Button("Close Left UI"))
            {
                Global.UI.Get<LayoutUI>().CloseLeft();
            }

            if (DebugGUI.Button("Open Right"))
            {
                Global.UI.Get<LayoutUI>().OpenRight();
            }

            if (DebugGUI.Button("Close Right"))
            {
                Global.UI.Get<LayoutUI>().CloseRight();
            }

            if (DebugGUI.Button("Open SettingUI"))
            {
                Global.UI.Open<SettingUI>();
            }
            if (DebugGUI.Button("Open Dialog 1"))
            {
                Global.UI.Open<DialogUI>((ui) => { ui.SetData(new Color(0.2f, 0, 0, 1)); }, Constant.LOCAL_RES_MODULE, 1);
            }

            if (DebugGUI.Button("Open Dialog 2"))
            {
                Global.UI.Open<DialogUI>((ui) => { ui.SetData(new Color(0, 0.2f, 0, 1)); }, Constant.LOCAL_RES_MODULE, 2);
            }

            if (DebugGUI.Button("Open Dialog 3"))
            {
                Global.UI.Open<DialogUI>((ui) => { ui.SetData(new Color(0, 0.5f, 0, 1)); }, Constant.LOCAL_RES_MODULE, 3);
            }

            if (DebugGUI.Button("Open Dialog 4"))
            {
                Global.UI.Open<DialogUI>((ui) => { ui.SetData(new Color(0, 0.8f, 0, 1)); }, Constant.COMMON_RES_MODULE, 4);
            }

        }

        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }
    }
}

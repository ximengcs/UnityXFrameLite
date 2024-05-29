using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Procedure;
using UnityXFrameLib.UIElements;
using Assets.Scripts.Test;
using XFrame.Core.Threads;
using XFrame.Core;
using XFrameShare.Network;
using XFrame.Tasks;
using Assets.Scripts;

namespace Game.Core.Procedure
{
    public class EnterGameProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            string langFile = Global.LocalRes.Load<TextAsset>(Constant.LANG_FILE_PATH).text;
            Global.I18N.Parse(langFile);
            Initialize();
            GameConst.Initialize();
        }

        private async void Initialize()
        {
            await UIModuleExt.CollectAutoTask();
            Global.UI.Open<ControllerUI>();
        }
    }
}

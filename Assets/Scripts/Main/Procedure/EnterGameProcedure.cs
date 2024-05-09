using XFrame.Core;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Local;
using UnityXFrameLib.Improve;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.Resource;
using Game.Test;
using XFrame.Modules.Resource;
using XFrame.Modules.Tasks;
using UnityXFrameLib.UIElements;
using Assets.Scripts.Test;

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
            //Entry.AddModule<GCModule>();
        }

        private async void Initialize()
        {
            await UIModuleExt.CollectAutoTask();
            Global.UI.Open<ControllerUI>();
        }
    }
}

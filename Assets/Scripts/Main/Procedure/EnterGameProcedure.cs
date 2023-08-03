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

namespace Game.Core.Procedure
{
    public class EnterGameProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("EnterGameProcedure");

            string langFile = Entry.GetModule<ResModule>(Constant.LOCAL_RES_MODULE)
                .Load<TextAsset>(Constant.LANG_FILE_PATH).text;
            LocalizeModule.Inst.Parse(langFile);
            Entry.AddModule<GCModule>();
        }
    }
}

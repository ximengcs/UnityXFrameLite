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

namespace Game.Core.Procedure
{
    public class EnterGameProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("EnterGameProcedure");

            string langFile = Module.LocalRes.Load<TextAsset>(Constant.LANG_FILE_PATH).text;
            Module.I18N.Parse(langFile);
            //Entry.AddModule<GCModule>();
        }
    }
}

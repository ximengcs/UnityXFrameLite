using XFrame.Core;
using UnityXFrameLib.Improve;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;

namespace Game.Core.Procedure
{
    public class EnterGameProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("EnterGameProcedure");
            Entry.AddModule<GCModule>();
        }
    }
}

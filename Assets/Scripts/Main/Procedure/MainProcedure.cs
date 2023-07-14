using UnityEngine;
using UnityXFrame.Core.Diagnotics;
using XFrame.Core;
using XFrame.Modules.Procedure;

namespace Game.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Entry.AddModule<Debuger>();
            Application.targetFrameRate = 60;
            ChangeState<EnterGameProcedure>();
            //ChangeState<CheckResUpdateProcedure>();
        }
    }
}

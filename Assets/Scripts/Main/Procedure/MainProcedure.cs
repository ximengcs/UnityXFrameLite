using UnityEngine;
using XFrame.Modules.Procedure;

namespace Game.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Application.targetFrameRate = 60;
            ChangeState<CheckResUpdateProcedure>();
        }
    }
}

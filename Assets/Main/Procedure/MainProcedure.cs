using XFrame.Modules.Procedure;

namespace Game.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            ChangeState<CheckResUpdateProcedure>();
        }
    }
}

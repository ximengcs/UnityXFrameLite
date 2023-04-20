using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.HotUpdate;

namespace XHotfix.Core
{
    public class HotfixMainProcedure : HotProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("Hello XFrame.");
        }
    }
}
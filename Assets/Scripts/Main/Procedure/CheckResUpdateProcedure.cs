using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.HotUpdate;

namespace Game.Core.Procedure
{
    public class CheckResUpdateProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("Start hot update check task.");
            HotUpdateCheckTask checkTask = new HotUpdateCheckTask();
            checkTask.OnCompleted(() =>
            {
                if (checkTask.Success)
                    Log.Debug($"Hot update check task has success.");
                else
                    Log.Debug("Hot update check task has failure.");
                Log.Debug("Start hot update download task.");
                HotUpdateDownTask downTask = new HotUpdateDownTask();
                downTask.AddList(checkTask.ResList).OnCompleted(() =>
                {
                    if (downTask.Success)
                        Log.Debug("Hot update download task has success.");
                    else
                        Log.Debug("Hot update download task has failure.");
                    ChangeState<EnterGameProcedure>();
                }).Coroutine();
            }).Coroutine();
        }
    }
}
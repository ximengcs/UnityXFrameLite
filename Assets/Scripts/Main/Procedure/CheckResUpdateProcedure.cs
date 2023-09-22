using UnityXFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.HotUpdate;
using XFrame.Core;

namespace Game.Core.Procedure
{
    public class CheckResUpdateProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("Start hot update check task.");
            HotUpdateCheckTask checkTask = Module.Task.GetOrNew<HotUpdateCheckTask>(Constant.UPDATE_CHECK_TASK);
            checkTask.OnComplete(() =>
            {
                if (checkTask.Success)
                    Log.Debug($"Hot update check task has success.");
                else
                    Log.Debug("Hot update check task has failure.");
                Log.Debug("Start hot update download task.");
                HotUpdateDownTask downTask = Module.Task.GetOrNew<HotUpdateDownTask>(Constant.UPDATE_RES_TASK);
                downTask.AddList(checkTask.ResList).OnComplete(() =>
                {
                    if (downTask.Success)
                        Log.Debug("Hot update download task has success.");
                    else
                        Log.Debug("Hot update download task has failure.");
                    ChangeState<EnterGameProcedure>();
                }).Start();
            }).Start();
        }
    }
}
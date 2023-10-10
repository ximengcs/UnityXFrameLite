using UnityEngine;
using UnityXFrame.Core;
using UnityXFrame.Core.Diagnotics;
using UnityXFrameLib.Diagnotics;
using UnityXFrameLib.Tasks;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Event;
using XFrame.Modules.Resource;
using XFrame.Modules.Tasks;

namespace Game.Test
{
    [DebugWindow]
    public class PoolTest : IDebugWindow
    {
        public void OnAwake()
        {
            
        }

        public void OnDraw()
        {
            DebugGUI.Label(Global.Task.ExecCount.ToString());
            if (DebugGUI.Button("HandlerInfo Listen"))
            {

            }
            if (DebugGUI.Button("HandlerInfo UnListen"))
            {

            }
            if (DebugGUI.Button("Res1"))
            {
                ITask task = Global.Res.LoadAsync<TextAsset>("Config/Perch.txt")
                    .OnComplete((TextAsset asset) => Log.Debug(asset.text))
                    .AutoDelete();
                task.Start();
                Log.Debug($"hash {task.GetHashCode()} {task.Name}");
            }
            if (DebugGUI.Button("Res2"))
            {
                ITask task = Global.Res.LoadAsync<TextAsset>("Config/Prop.csv")
                    .OnComplete((TextAsset asset) => Log.Debug(asset.text))
                    .AutoDelete();
                task.Start();
                Log.Debug($"hash {task.GetHashCode()} {task.Name}");
            }
            if (DebugGUI.Button("Action1"))
            {
                ITask t1 = TaskExt.Invoke(() =>
                {
                    Log.Debug(1);
                    ITask t2 = TaskExt.Invoke(() =>
                    {
                        Log.Debug(2);
                        ITask t3 = TaskExt.NextFrame(() =>
                        {
                            Log.Debug(3);
                        }).AutoDelete();
                        Log.Debug(t3.GetHashCode());
                    }).AutoDelete();
                    Log.Debug(t2.GetHashCode());
                }).AutoDelete();
                Log.Debug(t1.GetHashCode());
            }
        }

        public void Dispose()
        {

        }
    }
}

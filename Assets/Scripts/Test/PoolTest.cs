using UnityEngine;
using UnityXFrame.Core.Diagnotics;
using XFrame.Modules.Diagnotics;

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
            if (DebugGUI.Button("HandlerInfo Listen"))
            {

            }
            if (DebugGUI.Button("HandlerInfo UnListen"))
            {

            }
            if (DebugGUI.Button("Res1"))
            {
                Global.Res.LoadAsync<TextAsset>("Config/Perch.txt")
                    .OnCompleted((TextAsset asset) => Log.Debug(asset.text))
                    .Coroutine();
            }
            if (DebugGUI.Button("Res2"))
            {
                Global.Res.LoadAsync<TextAsset>("Config/Prop.csv")
                    .OnCompleted((TextAsset asset) => Log.Debug(asset.text))
                    .Coroutine();
            }
            if (DebugGUI.Button("Action1"))
            {

            }
        }

        public void Dispose()
        {

        }
    }
}

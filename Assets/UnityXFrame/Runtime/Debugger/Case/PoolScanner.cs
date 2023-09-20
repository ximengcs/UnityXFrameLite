
using UnityEngine;
using XFrame.Modules.Pools;
using XFrame.Utility;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugWindow(-1001, "Pool")]
    public class PoolScanner : IDebugWindow
    {
        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            GUILayout.BeginHorizontal();
            DebugGUI.Title("TypeName");
            DebugGUI.Title("U", DebugGUI.Width(70));
            DebugGUI.Title("C", DebugGUI.Width(70));
            GUILayout.EndHorizontal();

            foreach (IPool pool in PoolModule.Inst.AllPool)
            {
                GUILayout.BeginHorizontal();
                DebugGUI.Label(TypeUtility.GetSimpleName(pool.ObjectType));
                DebugGUI.Label(pool.UseCount.ToString(), DebugGUI.Width(70));
                DebugGUI.Label(pool.ObjectCount.ToString(), DebugGUI.Width(70));
                GUILayout.EndHorizontal();
            }
        }

        public void Dispose()
        {

        }
    }
}

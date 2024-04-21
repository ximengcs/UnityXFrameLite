using UnityEngine;
using XFrame.Utility;
using System.Collections.Generic;
using XFrame.Modules.Caches;
using XFrame.Core;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugWindow(-1000, "Cache")]
    public class CacheScanner : IDebugWindow
    {
        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            GUILayout.BeginHorizontal();
            DebugGUI.Title("TypeName");
            DebugGUI.Title("C", DebugGUI.Width(70));
            GUILayout.EndHorizontal();

            ICollection<XCache.ObjectCollection> caches = Entry.GetModule<XCache>().Collections;
            foreach (var cache in caches)
            {
                GUILayout.BeginHorizontal();
                DebugGUI.Label(TypeUtility.GetSimpleName(cache.TargetType));
                DebugGUI.Label(cache.Count.ToString(), DebugGUI.Width(70));
                GUILayout.EndHorizontal();
            }
        }

        public void Dispose()
        {

        }
    }
}

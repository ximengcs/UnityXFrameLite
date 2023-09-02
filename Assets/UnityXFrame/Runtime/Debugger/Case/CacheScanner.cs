using UnityEngine;
using XFrame.Core.Caches;
using System.Collections.Generic;
using XFrame.Utility;

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
            ICollection<XCache.ObjectCollection> caches = XCache.Collections;
            foreach (var cache in caches)
            {
                GUILayout.BeginHorizontal();
                DebugGUI.Label(TypeUtility.GetSimpleName(cache.TargetType.FullName));
                DebugGUI.Label(cache.Count.ToString(), DebugGUI.Width(100));
                GUILayout.EndHorizontal();
            }
        }

        public void Dispose()
        {

        }
    }
}

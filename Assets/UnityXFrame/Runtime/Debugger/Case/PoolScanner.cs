using System;
using UnityEngine;
using XFrame.Utility;
using XFrame.Modules.Pools;
using System.Collections.Generic;
using XFrame.Core;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugWindow(-1001, "Pool")]
    public class PoolScanner : IDebugWindow
    {
        private class ItemInfo
        {
            public bool Display;
            public Vector2 Pos;
        }

        private Dictionary<Type, ItemInfo> m_DisplayList;

        public void OnAwake()
        {
            m_DisplayList = new Dictionary<Type, ItemInfo>();
        }

        public void OnDraw()
        {
            GUILayout.BeginHorizontal();
            DebugGUI.Title("TypeName");
            DebugGUI.Title("U", DebugGUI.Width(70));
            DebugGUI.Title("C", DebugGUI.Width(70));
            DebugGUI.Title(" ", DebugGUI.Width(70));
            GUILayout.EndHorizontal();

            foreach (IPool pool in XModule.Pool.AllPool)
            {
                GUILayout.BeginHorizontal();
                DebugGUI.Label(TypeUtility.GetSimpleName(pool.ObjectType));
                DebugGUI.Label(pool.UseCount.ToString(), DebugGUI.Width(70));
                DebugGUI.Label(pool.ObjectCount.ToString(), DebugGUI.Width(70));

                Type target = pool.ObjectType;
                m_DisplayList.TryGetValue(target, out ItemInfo info);
                if (DebugGUI.Button("↓", DebugGUI.Width(70)))
                {
                    if (info != null)
                    {
                        info.Display = !info.Display;
                    }
                    else
                    {
                        info = new ItemInfo();
                        info.Display = true;
                        m_DisplayList.Add(target, info);
                    }
                }
                GUILayout.EndHorizontal();

                if (info != null && info.Display)
                {
                    DebugGUI.BeginVertical();
                    info.Pos = DebugGUI.BeginScrollView(info.Pos, DebugGUI.Height(200));
                    foreach (IPoolObject obj in pool.AllObjects)
                    {
                        string name = obj.MarkName;
                        if (!string.IsNullOrEmpty(name))
                            DebugGUI.Label(name);
                    }
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                }
            }
        }

        public void Dispose()
        {

        }
    }
}

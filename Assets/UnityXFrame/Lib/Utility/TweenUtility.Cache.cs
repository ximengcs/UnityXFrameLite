using UnityEngine;
using System.Collections.Generic;

namespace UnityXFrameLib.Utilities
{
    public partial class TweenUtility
    {
        private static List<PathInfo> m_PathCache = new List<PathInfo>();

        private static void InnerSaveCache(Vector2 start, Vector2 end, float degree, Vector3[] points)
        {
            m_PathCache.Add(new PathInfo(start, end, degree, points));
        }

        private static bool InnerLoadCache(Vector2 start, Vector2 end, float degree, out Vector3[] points)
        {
            foreach (PathInfo info in m_PathCache)
            {
                if (info.Is(start, end, degree))
                {
                    points = info.Points;
                    return true;
                }
            }
            points = null;
            return false;
        }
    }
}

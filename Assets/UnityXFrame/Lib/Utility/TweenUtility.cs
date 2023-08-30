using DG.Tweening;
using UnityEngine;
using DG.Tweening.Plugins.Core.PathCore;

namespace UnityXFrameLib.Utilities
{
    public partial class TweenUtility
    {
        public static Path _1(Vector2 start, Vector2 end, float degree)
        {
            if (InnerLoadCache(start, end, degree, out Vector3[] points))
            {
                return new Path(PathType.CubicBezier, points, 1);
            }
            else
            {
                float rad = Mathf.Deg2Rad * degree;
                float k = (end.y - start.y) / (end.x - start.x);
                float baseX = start.x - (end.x - start.x) / 3;
                float baseB = end.y - k * end.x;
                float baseY = k * baseX + baseB;
                Vector2 baseVec = new Vector2(baseX, baseY);
                Vector2 bottomVec = baseVec - start;
                bottomVec = new Vector2(bottomVec.x * Mathf.Cos(rad) - bottomVec.y * Mathf.Sin(rad),
                    bottomVec.x * Mathf.Sin(rad) + bottomVec.y * Mathf.Cos(rad)) + start;
                float bottomK = (start.y - bottomVec.y) / (start.x - bottomVec.x);

                float x2 = end.x - (start.x - bottomVec.x) * 2;
                float x2b = end.y - bottomK * end.x;
                float y2 = bottomK * x2 + x2b;
                points = new Vector3[] { end, bottomVec, new Vector3(x2, y2) };
                Path path = new Path(PathType.CubicBezier, points, 1);
                InnerSaveCache(start, end, degree, points);
                return path;
            }
        }
    }
}

using UnityEngine;

namespace UnityXFrameLib.Utilities
{
    public partial class TweenUtility
    {
        private struct PathInfo
        {
            public Vector2 Start;
            public Vector2 End;
            public float Degree;
            public Vector3[] Points;

            public PathInfo(Vector2 start, Vector2 end, float degree, Vector3[] points)
            {
                Start = start;
                End = end;
                Degree = degree;
                Points = points;
            }

            public bool Is(Vector2 start, Vector2 end, float degree)
            {
                return start == Start && end == End && Mathf.Approximately(degree, Degree);
            }
        }
    }
}

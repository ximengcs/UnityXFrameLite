using System.Diagnostics;
using UnityEngine.Scripting;
using XFrame.Tasks;

namespace UnityXFrameLib.Improve
{
    public class GCTask : XProTask
    {
        public GCTask() : base(null)
        {
            m_ProHandler = new Handler();
        }

        private class Handler : IProTaskHandler
        {
            private float m_Pro;
#if !UNITY_EDITOR
            private Stopwatch m_Watch;
#endif

            public object Data => throw new System.NotImplementedException();

            public bool IsDone
            {
                get
                {
#if UNITY_EDITOR
                    m_Pro = XTaskHelper.MAX_PROGRESS;
#else
                    m_Watch.Restart();
                    bool finish = !GarbageCollector.CollectIncremental();
                    m_Watch.Stop();
                    if (finish || m_Watch.ElapsedMilliseconds == 0)
                        m_Pro = XTaskHelper.MAX_PROGRESS;
                    else
                        InnerFakePro();
#endif
                    return m_Pro >= XTaskHelper.MAX_PROGRESS;
                }
            }

            public float Pro => throw new System.NotImplementedException();

            public Handler()
            {

#if !UNITY_EDITOR
                m_Watch = new Stopwatch();
#endif
            }

            public void OnCancel()
            {
                throw new System.NotImplementedException();
            }

            private void InnerFakePro()
            {
                m_Pro += (XTaskHelper.MAX_PROGRESS - m_Pro) * 0.1f;
                if (m_Pro >= 0.999f)
                    m_Pro = XTaskHelper.MAX_PROGRESS;
            }

        }
    }
}

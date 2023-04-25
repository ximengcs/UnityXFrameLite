using System.Diagnostics;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;
using UnityEngine.Scripting;

namespace UnityXFrameLib.Improve
{
    public class GCTask : ProActionTask
    {
        private float m_Pro;

        protected override void OnInit()
        {
            base.OnInit();
            m_Pro = 0;
            Add(InnerStart);
        }

        private float InnerStart()
        {
#if UNITY_EDITOR
            return MAX_PRO;
#else
            Stopwatch sw = Stopwatch.StartNew();
            bool finish = !GarbageCollector.CollectIncremental();
            sw.Stop();
            UnityEngine.Debug.LogWarning("gc " + sw.ElapsedMilliseconds + " " + finish + " " + m_Pro);
            if (finish || sw.ElapsedMilliseconds == 0)
                return MAX_PRO;

            InnerFakePro();
            return m_Pro;
#endif
        }

        private void InnerFakePro()
        {
            m_Pro += (MAX_PRO - m_Pro) * 0.1f;
            if (m_Pro >= 0.999f)
                m_Pro = MAX_PRO;
        }
    }
}

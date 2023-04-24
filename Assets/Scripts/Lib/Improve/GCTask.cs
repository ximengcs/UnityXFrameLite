using System;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

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
            if (m_Pro == 0)
                GC.Collect();
            m_Pro += TimeModule.Inst.EscapeTime;
            return m_Pro;
        }
    }
}

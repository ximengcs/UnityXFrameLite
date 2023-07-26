using UnityEngine;
using XFrame.Modules.Archives;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugHelp("common tools")]
    [DebugWindow(-996)]
    public class CommonCase : IDebugWindow
    {
        private bool m_TimerCD;
        private bool m_LockFPS;
        private ITask m_TimerDebugTask;

        public void Dispose()
        {

        }

        public void OnAwake()
        {
            m_LockFPS = true;
        }

        public void OnDraw()
        {
            GUILayout.BeginHorizontal();
            DebugGUI.Label("Debug Timer CD");
            bool timerCD = DebugGUI.Power(m_TimerCD);
            if (timerCD && m_TimerDebugTask == null)
            {
                m_TimerDebugTask = TaskModule.Inst.GetOrNew<RepeatActionTask>().Add(1.0f, InnerTestTimerCD);
                m_TimerDebugTask.Start();
            }
            m_TimerCD = timerCD;
            GUILayout.EndHorizontal();

            if (DebugGUI.Button("Clear User Data"))
            {
                CmdList.Inst.clear();
            }

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Lock FPS");
            bool lockFPS = DebugGUI.Power(m_LockFPS);
            if (lockFPS != m_LockFPS)
            {
                m_LockFPS = lockFPS;
                if (m_LockFPS)
                    Application.targetFrameRate = 60;
                else
                    Application.targetFrameRate = 0;
            }
            GUILayout.EndHorizontal();
        }

        private bool InnerTestTimerCD()
        {
            foreach (CDTimer timer in TimeModule.Inst.GetTimers())
            {
                float t = timer.CheckTime();
                Log.Debug("Timer", $"{timer.Name} {(t > 0 ? t : "has reach")}");
            }
            bool finish = !m_TimerCD;
            if (finish)
                m_TimerDebugTask = null;
            return finish;
        }
    }
}
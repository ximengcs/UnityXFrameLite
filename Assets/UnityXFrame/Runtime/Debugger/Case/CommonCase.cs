using UnityEngine;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugHelp("common tools")]
    [DebugWindow(-996)]
    public class CommonCase : IDebugWindow
    {
        private bool m_TimerCD;
        private bool m_LockFPS;
        private int m_FPS;
        private int m_FPSMin = 10;
        private int m_FPSMax = 144;
        private ITask m_TimerDebugTask;

        public void Dispose()
        {

        }

        public void OnAwake()
        {
            m_LockFPS = true;
            m_FPS = Mathf.Clamp(m_FPS, m_FPSMin, m_FPSMax);
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

            DebugGUI.BeginVertical();
            GUILayout.BeginHorizontal();
            m_FPS = (int)DebugGUI.Slider(m_FPS, m_FPSMin, m_FPSMax);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DebugGUI.Label($"Lock FPS {m_FPS}");
            bool lockFPS = DebugGUI.Power(m_LockFPS);
            if (lockFPS != m_LockFPS)
            {
                m_LockFPS = lockFPS;
                if (m_LockFPS)
                    Application.targetFrameRate = 60;
                else
                    Application.targetFrameRate = m_FPS;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (DebugGUI.Button("Clear User Data"))
            {
                CmdList.clear_user_data();
            }
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
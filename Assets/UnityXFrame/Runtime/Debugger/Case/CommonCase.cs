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
        private float m_TimeScale;
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
            m_TimeScale = Time.timeScale;
        }

        public void OnDraw()
        {
            GUILayout.BeginHorizontal();
            DebugGUI.Label("Debug Timer CD");
            bool timerCD = DebugGUI.Power(m_TimerCD);
            if (timerCD && m_TimerDebugTask == null)
            {
                m_TimerDebugTask = Global.Task.GetOrNew<ActionTask>().Add(1.0f, InnerTestTimerCD);
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

            GUILayout.BeginHorizontal();
            DebugGUI.Label("Time Scale", GUILayout.Width(Global.Debugger.FitWidth(200)));
            DebugGUI.Label($"{m_TimeScale}", GUILayout.Width(Global.Debugger.FitWidth(80)));
            m_TimeScale = DebugGUI.Slider(m_TimeScale, 0, 5);
            if (m_TimeScale >= 1)
                m_TimeScale = (int)m_TimeScale;
            else if (m_TimeScale >= 0.5f)
                m_TimeScale = 0.5f;
            else
                m_TimeScale = 0.25f;
            if (m_TimeScale != Time.timeScale)
                Time.timeScale = m_TimeScale;
            GUILayout.EndHorizontal();

            if (DebugGUI.Button("Clear User Data"))
            {
                CmdList.clear_user_data();
            }
        }

        private bool InnerTestTimerCD()
        {
            foreach (CDTimer timer in Global.Time.GetTimers())
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
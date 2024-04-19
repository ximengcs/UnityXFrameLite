using System.Threading.Tasks;
using UnityEngine;
using UnityXFrame.Core.Diagnotics;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.NewTasks;

namespace Game.Test
{
    [DebugWindow()]
    public class TaskCase : IDebugWindow
    {
        public void OnAwake()
        {
        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Task1"))
            {
                m_Task1 = Task1();
                m_Task1
                    //.SetAction(XTaskAction.ContinueWhenSubTaskFailure)
                    .OnCompleted((state) => { Log.Debug($"task1 complete {state}"); })
                    .Coroutine();
            }

            if (DebugGUI.Button("Task1 Calcel"))
            {
                m_Task1.Cancel(true);
            }

            if (DebugGUI.Button("Task2 Calcel"))
            {
                m_Task2.Cancel(true);
            }

            if (DebugGUI.Button("Task3 Calcel"))
            {
                m_Task3.Cancel(true);
            }
        }

        private XTask m_Task1;

        private async XTask Task1()
        {
            Log.Debug(1);
            await Task.Delay(1000);
            Log.Debug(2);
            await Task.Delay(2000);
            Log.Debug(3);
            await Task.Delay(3000);
            m_Task2 = Task2();
            m_Task2.OnCompleted((state) => { Log.Debug($"task2 complete {state}"); });
            await m_Task2;
            Log.Debug("4");
            await Task.Delay(1000);
            Log.Debug("5");
        }

        private XTask m_Task2;

        private async XTask Task2()
        {
            Log.Debug("2 1");
            await Task.Delay(1000);
            Log.Debug("2 2");
            await Task.Delay(2000);
            Log.Debug("2 3");
            await Task.Delay(3000);
            m_Task3 = Task3();
            m_Task3.OnCompleted((state) => Log.Debug($"task 3 complete {state}"));
            await m_Task3;
            Log.Debug(m_Task3.GetResult());
        }

        private XTask<int> m_Task3;

        private async XTask<int> Task3()
        {
            Log.Debug("3 1");
            await Task.Delay(1000);
            return 9;
        }

        public void Dispose()
        {
        }
    }
}
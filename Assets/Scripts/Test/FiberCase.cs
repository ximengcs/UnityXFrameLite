
using System.Threading;
using UnityEngine;
using UnityXFrame.Core.Diagnotics;
using XFrame.Core.Threads;
using XFrame.Tasks;

namespace Assets.Scripts.Test
{
    [DebugWindow()]
    public class FiberCase : IDebugWindow
    {
        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Main"))
            {
                Global.Fiber.MainFiber.Post(MainTest, null);
            }
            if (DebugGUI.Button("Net"))
            {
                Global.Fiber.Get(1).Post(NetTest, null);
            }
            if (DebugGUI.Button("Main Async"))
            {
                Fiber current = Global.Fiber.MainFiber.Use();
                MainAsyncTest(null);
                current.Use();

            }
            if (DebugGUI.Button("Net Async"))
            {
                Fiber current = Global.Fiber.Get(1).Use();
                Global.Fiber.Get(1).Post(NetAsyncTest, null);
                current.Use();
            }
            if (DebugGUI.Button("Test Net To Main"))
            {
                Global.Fiber.Get(1).Post(TestNetToMain, null);
            }
        }

        private async void TestNetToMain(object state)
        {
            Debug.LogWarning($"Run Task, thread {Thread.CurrentThread.ManagedThreadId}");
            await XTask.Delay(1);
            Global.Fiber.MainFiber.Post((state) =>
            {
                int num = (int)state;
                Debug.LogWarning($"Receive Task, thread {Thread.CurrentThread.ManagedThreadId}");
            }, 98259);
        }

        private void MainTest(object state)
        {
            Debug.LogWarning($"Main Run, thread {Thread.CurrentThread.ManagedThreadId}");
        }

        private void NetTest(object state)
        {
            Debug.LogWarning($"Net Run, thread {Thread.CurrentThread.ManagedThreadId}");
        }

        private async void MainAsyncTest(object state)
        {
            Debug.LogWarning($"Main Run 1, thread {Thread.CurrentThread.ManagedThreadId}");
            var task = XTask.Delay(1);
            task.OnUpdate((pro) =>
            {
                Debug.Log(pro);
            });
            await task;

            Debug.LogWarning($"Main Run 2, thread {Thread.CurrentThread.ManagedThreadId}");
            await XTask.Delay(1);

            Debug.LogWarning($"Main Run 3, thread {Thread.CurrentThread.ManagedThreadId}");
            await XTask.Delay(1);

            Debug.LogWarning($"Main Run 4, thread {Thread.CurrentThread.ManagedThreadId}");
            await XTask.Delay(1);
        }

        private async void NetAsyncTest(object state)
        {
            Debug.LogWarning($"Net Run 1, thread {Thread.CurrentThread.ManagedThreadId}");
            await XTask.Delay(1);

            Debug.LogWarning($"Net Run 2, thread {Thread.CurrentThread.ManagedThreadId}");
            await XTask.Delay(1);

            Debug.LogWarning($"Net Run 3, thread {Thread.CurrentThread.ManagedThreadId}");
            await XTask.Delay(1);

            Debug.LogWarning($"Net Run 4, thread {Thread.CurrentThread.ManagedThreadId}");
            await XTask.Delay(1);
        }
    }
}

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityXFrame.Core;
using UnityXFrame.Core.Diagnotics;
using XFrame.Modules.Diagnotics;
using XFrame.Tasks;

namespace Game.Test
{
    [DebugWindow()]
    public class TaskCase : IDebugWindow
    {
        private bool m_Complete;

        public void OnAwake()
        {
        }

        private async XTask<int> Test()
        {
            XProTask task = XTask.Condition(() => m_Complete);
            await task;
            return 1;
        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Beat"))
            {
                Test().OnCompleted(() => Log.Debug("Complete")).Coroutine();
                XTask.Delay(1).OnCompleted(() => m_Complete = true).Coroutine();
            }
            if (DebugGUI.Button("Task1"))
            {
                m_Task1 = Task1();
                m_Task1.OnCompleted((state) => { Log.Debug($"task1 complete {state}"); }).Coroutine();
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

            if (DebugGUI.Button("Task 4"))
            {
                m_Task4 = Task4();
                m_Task4.OnCompleted((state) => { Log.Debug($"Task4 complete {state}"); });
            }

            if (DebugGUI.Button("Cancel Task 4"))
            {
                m_Task4.Cancel(true);
            }

            if (DebugGUI.Button("Cancel pro Task 4"))
            {
                m_ProTask4.Cancel(true);
            }

            if (DebugGUI.Button("DelayTest"))
            {
                InnerTestDelay().Coroutine();
            }

            if (DebugGUI.Button("Load Res"))
            {
                InnerTestWaitForCompletion();
                //InnerLoadResTask().Coroutine();
            }

            if (DebugGUI.Button("Load Res Atlas"))
            {
                InnerLoadResTaskAtlas().Coroutine();
            }
        }

        private async XTask InnerLoadResTaskAtlas()
        {
            Global.SpriteAtlas.AddEntry(Global.Res.Load<TextAsset>("Config/atlas_map.txt").text);
            Sprite sprite = await Global.Res.LoadAsync<Sprite>($"Assets/Data/Textures/QQQ/iloveu.png");
            new GameObject().AddComponent<SpriteRenderer>().sprite = sprite;
        }

        private async XTask InnerLoadResTask()
        {
            Sprite sprite = await Global.Res.LoadAsync<Sprite>($"Data2/Textures/QQQ/test2.png");
            new GameObject().AddComponent<SpriteRenderer>().sprite = sprite;

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>($"Data/UI/DialogUI.prefab");
            handle.WaitForCompletion();
            //GameObject obj = Global.Res.Load<GameObject>($"Data/UI/DialogUI.prefab");
            //Log.Debug(obj == null);
        }

        private void InnerTestWaitForCompletion()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>($"Data/UI/DialogUI.prefab");
            handle.WaitForCompletion();
        }

        private async XTask InnerTestDelay()
        {
            Log.Debug("1");
            XProTask task = XTask.Delay(2);
            await task;
            Log.Debug("2");
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
            m_Task3.OnCompleted((int state) => Log.Debug($"task 3 complete {state}"));
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

        private XTask m_Task4;
        private XProTask m_ProTask4;

        private async XTask Task4()
        {
            Log.Debug("Start");
            m_ProTask4 = new XProTask(new DOTweenProgress());
            //m_ProTask4.OnUpdate((pro) => Debug.LogWarning($"Pro {pro}"));
            m_ProTask4.OnCompleted((object state) => Log.Debug($"pro task 4 OnComplete {state}"));
            await m_ProTask4;
            Log.Debug("End");
        }


        public void Dispose()
        {
        }
    }
}
using XFrame.Core;
using UnityEngine;
using XFrame.Modules.Archives;
using UnityXFrame.Core.Diagnotics;
using XFrame.Modules.Config;

namespace UnityXFrame.Core
{
    public class Init : SingletonMono<Init>
    {
        public InitData Data;

        private void Awake()
        {
            XConfig.UseClassModule = new string[] { "Assembly-CSharp", "UnityXFrame", "UnityXFrameLib" };
            Entry.Init();
        }

        private void Start()
        {
            Entry.Start();
        }

        private void Update()
        {
            Entry.Update(Time.deltaTime);
        }

        private void OnGUI()
        {
#if CONSOLE
            Debuger.Inst?.OnGUI();
#endif
        }

        private void OnDestroy()
        {
            Entry.ShutDown();
        }

        private void OnApplicationFocus(bool focus)
        {
            ArchiveModule.Inst?.Save();
        }

        private void OnApplicationPause(bool pause)
        {
            ArchiveModule.Inst?.Save();
        }

        private void OnApplicationQuit()
        {
            ArchiveModule.Inst?.Save();
        }
    }
}
using XFrame.Core;
using UnityEngine;
using XFrame.Modules.Archives;
using UnityXFrame.Core.Diagnotics;
using XFrame.Modules.Config;

namespace UnityXFrame.Core
{
    [DefaultExecutionOrder(Constant.EXECORDER_INIT)]
    public class Init : SingletonMono<Init>
    {
        public InitData Data;

        private void Awake()
        {
            XConfig.Entrance = Data.Entrance;
            XConfig.DefaultRes = Data.ResMode;
            XConfig.DefaultLogger = Data.Logger;
            XConfig.ArchivePath = Constant.ArchivePath;
            XConfig.DefaultDownloadHelper = Data.DownloadHelper;
            XConfig.UseClassModule = new string[] { "Assembly-CSharp", "UnityXFrame", "UnityXFrame.Lib" };
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
            Debuger.Inst?.OnGUI();
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
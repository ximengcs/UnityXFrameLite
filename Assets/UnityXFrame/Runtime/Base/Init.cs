using XFrame.Core;
using UnityEngine;
using XFrame.Modules.Config;
using XFrame.Modules.Archives;
using System.Collections;

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
            XConfig.ArchiveUtilityHelper = Data.ArchiveUtilityHelper;
            XConfig.DefaultDownloadHelper = Data.DownloadHelper;
            XConfig.UseClassModule = Constant.TYPESYSTEM_MODULE;
            Entry.Init();
        }

        private void Start()
        {
            Entry.Start();
        }

        private void Update()
        {
            Entry.Trigger<IUpdater>(Time.deltaTime);
            if (!Global.EndOfFrame.Empty)
                StartCoroutine(InnerEndOfFrameHandler());
        }

        private IEnumerator InnerEndOfFrameHandler()
        {
            yield return Global.EndOfFrame.WaitYield;
            Entry.Trigger<IEndOfFrame>();
        }

        private void OnGUI()
        {
            Entry.Trigger<IGUI>();
        }

        private void OnApplicationFocus(bool focus)
        {
            Entry.Trigger<ISaveable>();
            Entry.Trigger<IAppFocus>();
        }

        private void OnApplicationPause(bool pause)
        {
            Entry.Trigger<ISaveable>();
        }

        private void OnApplicationQuit()
        {
            Entry.Trigger<ISaveable>();
        }

        private void OnDestroy()
        {
            Entry.ShutDown();
        }
    }
}
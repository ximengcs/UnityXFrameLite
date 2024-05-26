using XFrame.Core;
using UnityEngine;
using XFrame.Modules.Config;
using XFrame.Modules.Archives;
using System.Collections;
using XFrame.Tasks;
using XFrame.Modules.Diagnotics;
using System;

namespace UnityXFrame.Core
{
    [DefaultExecutionOrder(Constant.EXECORDER_INIT)]
    public class Init : SingletonMono<Init>
    {
        private EndOfFrameModule m_EndOfFrame;

        public InitData Data;

        private void Awake()
        {
            XTaskHelper.ExceptionHandler += Log.Exception;
            InnerConfigType();
            XConfig.Entrance = Data.Entrance;
            XConfig.DefaultRes = Data.ResMode;
            XConfig.DefaultLogger = Data.Logger;
            XConfig.ArchivePath = Constant.ArchivePath;
            XConfig.ArchiveUtilityHelper = Data.ArchiveUtilityHelper;
            XConfig.DefaultDownloadHelper = Data.DownloadHelper;
            XConfig.TypeChecker = new TypeChecker();

            Entry.Init();
            Global.Fiber.MainFiber.Use();
        }

        private void InnerConfigType()
        {
            TypeChecker.IncludeModule("Assembly-CSharp");
            TypeChecker.IncludeModule("XFrameShare");
            TypeChecker.IncludeModule("UnityXFrame");
            TypeChecker.IncludeModule("UnityXFrame.Lib");
            TypeChecker.ExcludeNameSpace("CommandLine");
            TypeChecker.ExcludeNameSpace("CommandLine.Core");
            TypeChecker.ExcludeNameSpace("CommandLine.Infrastructure");
            TypeChecker.ExcludeNameSpace("CommandLine.Text");
            TypeChecker.ExcludeNameSpace("CSharpx");
            TypeChecker.ExcludeNameSpace("CsvHelper");
            TypeChecker.ExcludeNameSpace("CsvHelper.Configuration");
            TypeChecker.ExcludeNameSpace("CsvHelper.Configuration.Attributes");
            TypeChecker.ExcludeNameSpace("CsvHelper.Delegates");
            TypeChecker.ExcludeNameSpace("CsvHelper.Expressions");
            TypeChecker.ExcludeNameSpace("CsvHelper.TypeConversion");
            TypeChecker.ExcludeNameSpace("RailwaySharp.ErrorHandling");
        }

        private void Start()
        {
            m_EndOfFrame = Entry.GetModule<EndOfFrameModule>();
            Entry.Start();
        }

        private void Update()
        {
            double escape = (double)Time.deltaTime;
            Entry.Trigger<IUpdater>(escape);
            if (!m_EndOfFrame.Empty)
                StartCoroutine(InnerEndOfFrameHandler());
        }

        private void FixedUpdate()
        {
            Entry.Trigger<IFinishUpdater>((double)Time.fixedDeltaTime);
        }

        private IEnumerator InnerEndOfFrameHandler()
        {
            yield return m_EndOfFrame.WaitYield;
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
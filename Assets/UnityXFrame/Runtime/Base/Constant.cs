﻿using System.IO;
using UnityEngine;

namespace UnityXFrame.Core
{
    public static class Constant
    {
        public const int EXECORDER_INIT = 100;
        public const int EXECORDER_AFTER = EXECORDER_INIT + 1;
        public const int EXECORDER_BEFORE = EXECORDER_INIT - 1;

        public static string ArchivePath => Path.Combine(Application.persistentDataPath, "archive");

        public static int SIZE_X = 1080;
        public static int SIZE_Y = 1920;
        public static int UNIT_PIXEL = 100;
        public static int SCENEUI_SORT_LAYER = 0;

        public const int JOSN_TYPE = 0;
        public const int CSV_TYPE = 1;

        public static string LANG_FILE_PATH = "Data/Lang/lang.csv";
        public static string AUDIO_PATH = "Assets/Data/Audio";
        public static string SCENEUI_RES_PATH = "Data/SceneUI";
        public static string UI_RES_PATH = "Data/UI";
        public static string CONFIG_PATH = "Data/Config";
        public static string MAIN_GROUPUI = "Main";

        public static string UPDATE_CHECK_TASK = "UpdateCheckTask";
        public static string UPDATE_RES_TASK = "UpdateResTask";

        public const int COMMON_RES_MODULE = 0;
        public const int LOCAL_RES_MODULE = 1;
    }
}

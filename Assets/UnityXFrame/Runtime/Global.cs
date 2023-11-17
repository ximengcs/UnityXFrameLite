using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.Resource;
using UnityXFrame.Core.UIElements;
using XFrame.Core;
using XFrame.Modules.Archives;
using XFrame.Modules.Conditions;
using XFrame.Modules.Containers;
using XFrame.Modules.Crypto;
using XFrame.Modules.Datas;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;
using XFrame.Modules.Entities;
using XFrame.Modules.Event;
using XFrame.Modules.ID;
using XFrame.Modules.Local;
using XFrame.Modules.Plots;
using XFrame.Modules.Pools;
using XFrame.Modules.Procedure;
using XFrame.Modules.Rand;
using XFrame.Modules.Reflection;
using XFrame.Modules.Resource;
using XFrame.Modules.Serialize;
using XFrame.Modules.StateMachine;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

namespace UnityXFrame.Core
{
    public static partial class Global
    {
        public static ITypeModule Type => XModule.Type;
        public static IArchiveModule Archive => XModule.Archive;
        public static IConditionModule Condition => XModule.Condition;
        public static IContainerModule Container => XModule.Container;
        public static ICryptoModule Crypto => XModule.Crypto;
        public static IDataModule Data => XModule.Data;
        public static IDownloadModule Download => XModule.Download;
        public static IEntityModule Entity => XModule.Entity;
        public static IEventModule Event => XModule.Event;
        public static IFsmModule Fsm => XModule.Fsm;
        public static IIdModule Id => XModule.Id;
        public static ILocalizeModule I18N => XModule.I18N;
        public static ILogModule Log => XModule.Log;
        public static IPlotModule Plot => XModule.Plot;
        public static IPoolModule Pool => XModule.Pool;
        public static IProcedureModule Procedure => XModule.Procedure;
        public static IRandModule Rand => XModule.Rand;
        public static IResModule Res => XModule.Res;
        public static IResModule LocalRes => m_LocalRes ??= Entry.GetModule<IResModule>(Constant.LOCAL_RES_MODULE);
        public static ISerializeModule Serialize => XModule.Serialize;
        public static ITaskModule Task => XModule.Task;
        public static ITimeModule Time => XModule.Time;
        public static IUIModule UI => m_UI ??= Entry.GetModule<IUIModule>();
        public static IAudioModule Audio => m_Audio ??= Entry.GetModule<IAudioModule>();
        public static IDebugger Debugger => m_Debugger ??= Entry.GetModule<IDebugger>();
        public static ISpriteAtlasModule SpriteAtlas => m_SpriteAtlas ??= Entry.GetModule<ISpriteAtlasModule>();

        internal static EndOfFrameModule EndOfFrame => m_EndOfFrame ??= Entry.GetModule<EndOfFrameModule>();
    }
}

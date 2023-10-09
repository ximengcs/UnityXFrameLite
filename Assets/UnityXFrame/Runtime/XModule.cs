using UnityXFrame.Core;
using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.UIElements;
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

namespace XFrame.Core
{
    public static partial class XModule
    {
        public static ITypeModule Type => ModuleUtility.Type;
        public static IArchiveModule Archive => ModuleUtility.Archive;
        public static IConditionModule Condition => ModuleUtility.Condition;
        public static IContainerModule Container => ModuleUtility.Container;
        public static ICryptoModule Crypto => ModuleUtility.Crypto;
        public static IDataModule Data => ModuleUtility.Data;
        public static IDownloadModule Download => ModuleUtility.Download;
        public static IEntityModule Entity => ModuleUtility.Entity;
        public static IEventModule Event => ModuleUtility.Event;
        public static IFsmModule Fsm => ModuleUtility.Fsm;
        public static IIdModule Id => ModuleUtility.Id;
        public static ILocalizeModule I18N => ModuleUtility.I18N;
        public static ILogModule Log => ModuleUtility.Log;
        public static IPlotModule Plot => ModuleUtility.Plot;
        public static IPoolModule Pool => ModuleUtility.Pool;
        public static IProcedureModule Procedure => ModuleUtility.Procedure;
        public static IRandModule Rand => ModuleUtility.Rand;
        public static IResModule Res => ModuleUtility.Res;
        public static IResModule LocalRes => m_LocalRes != null ? m_LocalRes : m_LocalRes = Entry.GetModule<ResModule>(Constant.LOCAL_RES_MODULE);
        public static ISerializeModule Serialize => ModuleUtility.Serialize;
        public static ITaskModule Task => ModuleUtility.Task;
        public static ITimeModule Time => ModuleUtility.Time;
        public static IUIModule UI => m_UI != null ? m_UI : m_UI = Entry.GetModule<IUIModule>();
        public static IAudioModule Audio => m_Audio != null ? m_Audio : m_Audio = Entry.GetModule<IAudioModule>();
        public static IDebugger Debugger => m_Debugger != null ? m_Debugger : m_Debugger = Entry.GetModule<IDebugger>();
    }
}

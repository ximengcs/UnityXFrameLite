using UnityXFrame.Core.Audios;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.Resource;
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
using XFrame.Modules.Times;

public static partial class Global
{
    private static ITypeModule m_Type;
    private static IArchiveModule m_Archive;
    private static IConditionModule m_Condition;
    private static IContainerModule m_Container;
    private static ICryptoModule m_Crypto;
    private static IDataModule m_Data;
    private static IDownloadModule m_Download;
    private static IEntityModule m_Entity;
    private static IEventModule m_Event;
    private static IFsmModule m_Fsm;
    private static IIdModule m_Id;
    private static ILocalizeModule m_I18N;
    private static ILogModule m_Log;
    private static IPlotModule m_Plot;
    private static IPoolModule m_Pool;
    private static IProcedureModule m_Procedure;
    private static IRandModule m_Rand;
    private static IResModule m_Res;
    private static ISerializeModule m_Serialize;
    private static ITimeModule m_Time;

    private static IResModule m_LocalRes;
    private static IUIModule m_UI;
    private static IAudioModule m_Audio;
    private static IDebugger m_Debugger;
    private static ISpriteAtlasModule m_SpriteAtlas;

    public static void Refresh()
    {
        m_Type = null;
        m_Archive = null;
        m_Condition = null;
        m_Container = null;
        m_Crypto = null;
        m_Data = null;
        m_Download = null;
        m_Entity = null;
        m_Event = null;
        m_Fsm = null;
        m_Id = null;
        m_I18N = null;
        m_Log = null;
        m_Plot = null;
        m_Pool = null;
        m_Procedure = null;
        m_Rand = null;
        m_Res = null;
        m_Serialize = null;
        m_Time = null;
        m_LocalRes = null;
        m_UI = null;
        m_Audio = null;
        m_Debugger = null;
    }
}

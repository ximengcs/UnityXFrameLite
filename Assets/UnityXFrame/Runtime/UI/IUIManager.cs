using System;
using UnityEngine;
using XFrame.Tasks;
using System.Collections;
using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace UnityXFrame.Core.UIElements
{
    public interface IUIManager
    {
        Canvas Canvas { get; }

        Vector2 PixelScale { get; }

        XTask PreloadResource(IEnumerable types, int useResModule);

        XTask PreloadResource(Type type, int useResModule);

        XTask Spwan(IEnumerable types, int useResModule);

        XTask Spwan(Type uiType, int useResModule);

        IUIGroup MainGroup { get; }

        IEventSystem Event { get; }

        IUI Open(Type uiType, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default);

        T Open<T>(OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default) where T : IUI;

        IUI Open(string uiName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default);

        IUI Open(string uiName, string groupName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default);

        T Open<T>(string groupName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default) where T : IUI;

        IUI Open(Type uiType, string groupName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default);

        IUI Open(Type uiType, IUIGroup group, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default);

        IUI Open(IUI ui, string groupName, OnDataProviderReady dataHandler = null);

        IUI Open(IUI ui, IUIGroup group, OnDataProviderReady dataHandler = null);

        T Open<T>(IUIGroup group, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default) where T : IUI;

        void Close<T>(int id = default) where T : IUI;

        void Close(Type uiType, int id = default);

        void Close(string uiName, int id = default);

        void Close(IUI ui);

        IUI Get(Type uiType, int id = default);

        T Get<T>(int id = default) where T : IUI;

        IUI Get(string uiName, int id = default);

        void DestroyUI(IUI ui);

        void DestroyUI<T>(int id = default) where T : IUI;

        void DestroyUI(Type type, int id = default);

        void DestroyUI(string uiName, int id = default);

        IUIGroup GetOrNewGroup(string groupName, int layer = -1);
    }
}

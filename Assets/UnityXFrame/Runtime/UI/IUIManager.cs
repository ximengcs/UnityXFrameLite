using System;
using UnityEngine;
using XFrame.Collections;
using XFrame.Modules.Event;
using XFrame.Modules.Tasks;
using XFrame.Modules.Containers;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIElements
{
    public interface IUIManager
    {
        Canvas Canvas { get; }

        Vector2 PixelScale { get; }

        ITask PreloadResource(Type[] types, int useResModule);

        ITask PreloadResource(IEnumerable<Type> types, int useResModule);

        ITask PreloadResource(IXEnumerable<Type> types, int useResModule);

        ITask Spwan(IEnumerable<Type> types, int useResModule);

        ITask Spwan(Type[] types, int useResModule);

        ITask Spwan(IXEnumerable<Type> types, int useResModule);

        ITask Spwan(Type uiType, int useResModule);

        ITask Spwan<T>(int useResModule) where T : IUI;

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

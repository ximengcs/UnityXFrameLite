using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFrame.Collections;
using XFrame.Core;
using XFrame.Modules.Containers;
using XFrame.Modules.Event;
using XFrame.Tasks;

namespace UnityXFrame.Core.UIElements
{
    public class CanvasUICom : UICom, IUIManager
    {
        private int m_ModuleId;
        private IUIModule m_UIModule;

        public int ModuleId
        {
            get
            {
                if (m_ModuleId == default)
                    m_ModuleId = UIHelper.ManagerId;
                return m_ModuleId;
            }
        }

        protected override void OnInit()
        {
            base.OnInit();

            string name = GetData<string>();
            RectTransform tf = m_UIFinder.GetUI<RectTransform>(name);
            GameObject inst = tf.gameObject;
            Canvas canvas = tf.GetComponent<Canvas>();
            if (canvas == null)
                canvas = inst.AddComponent<Canvas>();
            if (inst.GetComponent<GraphicRaycaster>() == null)
                inst.AddComponent<GraphicRaycaster>();

            UIModule.Data moduleData = new UIModule.Data(m_Module.Domain.GetModule<IEventModule>().NewSys(), canvas);
            m_UIModule = Entry.AddModule<UIModule>(ModuleId, moduleData);
            ClearData();
        }

        public Canvas Canvas => m_UIModule.Canvas;

        public Vector2 PixelScale => m_UIModule.PixelScale;

        public XTask PreloadResource(Type[] types, int useResModule)
        {
            return m_UIModule.PreloadResource(types, useResModule);
        }

        public XTask PreloadResource(IEnumerable<Type> types, int useResModule)
        {
            return m_UIModule.PreloadResource(types, useResModule);
        }

        public XTask PreloadResource(IXEnumerable<Type> types, int useResModule)
        {
            return m_UIModule.PreloadResource(types, useResModule);
        }

        public XTask PreloadResource(Type type, int useResModule)
        {
            return m_UIModule.PreloadResource(type, useResModule);
        }

        public XTask Spwan(IEnumerable<Type> types, int useResModule)
        {
            return m_UIModule.Spwan(types, useResModule);
        }

        public XTask Spwan(Type[] types, int useResModule)
        {
            return m_UIModule.Spwan(types, useResModule);
        }

        public XTask Spwan(IXEnumerable<Type> types, int useResModule)
        {
            return m_UIModule.Spwan(types, useResModule);
        }

        public XTask Spwan(Type uiType, int useResModule)
        {
            return m_UIModule.Spwan(uiType, useResModule);
        }

        public XTask Spwan<T>(int useResModule) where T : IUI
        {
            return m_UIModule.Spwan<T>(useResModule);
        }

        public IUIGroup MainGroup => m_UIModule.MainGroup;

        public IEventSystem Event => m_UIModule.Event;

        public IUI Open(Type uiType, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE,
            int id = default)
        {
            return m_UIModule.Open(uiType, dataHandler, useResModule, id);
        }

        public T Open<T>(OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default) where T : IUI
        {
            return m_UIModule.Open<T>(dataHandler, useResModule, id);
        }

        public IUI Open(string uiName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE,
            int id = default)
        {
            return m_UIModule.Open(uiName, dataHandler, useResModule, id);
        }

        public IUI Open(string uiName, string groupName, OnDataProviderReady dataHandler = null,
            int useResModule = Constant.COMMON_RES_MODULE, int id = default)
        {
            return m_UIModule.Open(uiName, groupName, dataHandler, useResModule, id);
        }

        public T Open<T>(string groupName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE,
            int id = default) where T : IUI
        {
            return m_UIModule.Open<T>(groupName, dataHandler, useResModule, id);
        }

        public IUI Open(Type uiType, string groupName, OnDataProviderReady dataHandler = null,
            int useResModule = Constant.COMMON_RES_MODULE, int id = default)
        {
            return m_UIModule.Open(uiType, groupName, dataHandler, useResModule, id);
        }

        public IUI Open(Type uiType, IUIGroup group, OnDataProviderReady dataHandler = null,
            int useResModule = Constant.COMMON_RES_MODULE, int id = default)
        {
            return m_UIModule.Open(uiType, group, dataHandler, useResModule, id);
        }

        public IUI Open(IUI ui, string groupName, OnDataProviderReady dataHandler = null)
        {
            return m_UIModule.Open(ui, groupName, dataHandler);
        }

        public IUI Open(IUI ui, IUIGroup group, OnDataProviderReady dataHandler = null)
        {
            return m_UIModule.Open(ui, group, dataHandler);
        }

        public T Open<T>(IUIGroup group, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE,
            int id = default) where T : IUI
        {
            return m_UIModule.Open<T>(group, dataHandler, useResModule, id);
        }

        public void Close<T>(int id = default) where T : IUI
        {
            m_UIModule.Close<T>(id);
        }

        public void Close(Type uiType, int id = default)
        {
            m_UIModule.Close(uiType, id);
        }

        public void Close(string uiName, int id = default)
        {
            m_UIModule.Close(uiName, id);
        }

        public void Close(IUI ui)
        {
            m_UIModule.Close(ui);
        }

        public IUI Get(Type uiType, int id = default)
        {
            return m_UIModule.Get(uiType, id);
        }

        public T Get<T>(int id = default) where T : IUI
        {
            return m_UIModule.Get<T>(id);
        }

        public IUI Get(string uiName, int id = default)
        {
            return m_UIModule.Get(uiName, id);
        }

        public void DestroyUI(IUI ui)
        {
            m_UIModule.DestroyUI(ui);
        }

        public void DestroyUI<T>(int id = default) where T : IUI
        {
            m_UIModule.DestroyUI<T>(id);
        }

        public void DestroyUI(Type type, int id = default)
        {
            m_UIModule.DestroyUI(type, id);
        }

        public void DestroyUI(string uiName, int id = default)
        {
            m_UIModule.DestroyUI(uiName, id);
        }

        public IUIGroup GetOrNewGroup(string groupName, int layer = -1)
        {
            return m_UIModule.GetOrNewGroup(groupName, layer);
        }
    }
}
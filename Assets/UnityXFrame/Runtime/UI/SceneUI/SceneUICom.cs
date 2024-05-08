using System;
using UnityEngine;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Event;
using XFrame.Modules.Tasks;
using XFrame.Modules.Containers;
using System.Collections.Generic;
using UnityXFrame.Core.Entities;
using XFrame.Modules.Entities;
using UnityEngine.UI;
using XFrame.Tasks;
using System.Collections;

namespace UnityXFrame.Core.UIElements
{
    public class SceneUICom : ShareEntity, IUIManager
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

        public Canvas Canvas => m_UIModule.Canvas;

        public Vector2 PixelScale => m_UIModule.PixelScale;

        public IUIGroup MainGroup => m_UIModule.MainGroup;

        public IEventSystem UIEvent => m_UIModule.Event;

        public void Close<T>(int id = 0) where T : IUI
        {
            m_UIModule.Close<T>(id);
        }

        public void Close(Type uiType, int id = 0)
        {
            m_UIModule.Close(uiType, id);
        }

        public void Close(string uiName, int id = 0)
        {
            m_UIModule.Close(uiName, id);
        }

        public void Close(IUI ui)
        {
            m_UIModule.Close(ui);
        }

        public void DestroyUI(IUI ui)
        {
            m_UIModule.DestroyUI(ui);
        }

        public void DestroyUI<T>(int id = 0) where T : IUI
        {
            m_UIModule.DestroyUI<T>(id);
        }

        public void DestroyUI(Type type, int id = 0)
        {
            m_UIModule.DestroyUI(type, id);
        }

        public void DestroyUI(string uiName, int id = 0)
        {
            m_UIModule.DestroyUI(uiName, id);
        }

        public IUI Get(Type uiType, int id = 0)
        {
            return m_UIModule.Get(uiType, id);
        }

        public T Get<T>(int id = 0) where T : IUI
        {
            return m_UIModule.Get<T>(id);
        }

        public IUI Get(string uiName, int id = 0)
        {
            return m_UIModule.Get(uiName, id);
        }

        public IUIGroup GetOrNewGroup(string groupName, int layer = -1)
        {
            return m_UIModule.GetOrNewGroup(groupName, layer);
        }

        public IUI Open(Type uiType, OnDataProviderReady dataHandler = null, int useResModule = 0, int id = 0)
        {
            return m_UIModule.Open(uiType, dataHandler, useResModule, id);
        }

        public T Open<T>(OnDataProviderReady dataHandler = null, int useResModule = 0, int id = 0) where T : IUI
        {
            return m_UIModule.Open<T>(dataHandler, useResModule, id);
        }

        public IUI Open(string uiName, OnDataProviderReady dataHandler = null, int useResModule = 0, int id = 0)
        {
            return m_UIModule.Open(uiName, dataHandler, useResModule, id);
        }

        public IUI Open(string uiName, string groupName, OnDataProviderReady dataHandler = null, int useResModule = 0,
            int id = 0)
        {
            return m_UIModule.Open(uiName, groupName, dataHandler, useResModule, id);
        }

        public T Open<T>(string groupName, OnDataProviderReady dataHandler = null, int useResModule = 0, int id = 0)
            where T : IUI
        {
            return m_UIModule.Open<T>(groupName, dataHandler, useResModule, id);
        }

        public IUI Open(Type uiType, string groupName, OnDataProviderReady dataHandler = null, int useResModule = 0,
            int id = 0)
        {
            return m_UIModule.Open(uiType, groupName, dataHandler, useResModule, id);
        }

        public IUI Open(Type uiType, IUIGroup group, OnDataProviderReady dataHandler = null, int useResModule = 0,
            int id = 0)
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

        public T Open<T>(IUIGroup group, OnDataProviderReady dataHandler = null, int useResModule = 0, int id = 0)
            where T : IUI
        {
            return m_UIModule.Open<T>(group, dataHandler, useResModule, id);
        }

        public XTask PreloadResource(IEnumerable types, int useResModule)
        {
            return m_UIModule.PreloadResource(types, useResModule);
        }

        public XTask PreloadResource(Type type, int useResModule)
        {
            return m_UIModule.PreloadResource(type, useResModule);
        }

        public XTask Spwan(IEnumerable types, int useResModule)
        {
            return m_UIModule.Spwan(types, useResModule);
        }

        public XTask Spwan(Type uiType, int useResModule)
        {
            return m_UIModule.Spwan(uiType, useResModule);
        }

        protected override void OnInit()
        {
            base.OnInit();

            GameObjectCom objCom = GetCom<GameObjectCom>();
            GameObject canvasInst = objCom.AddChild("SceneUICanvas");
            RectTransform tf = canvasInst.AddComponent<RectTransform>();
            Canvas canvas = canvasInst.AddComponent<Canvas>();
            canvasInst.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            Camera camera = GetData<Camera>();
            canvas.worldCamera = camera;
            tf.sizeDelta = new Vector2(Screen.width, Screen.height);
            tf.localScale = Vector3.one * camera.orthographicSize * 2 / Screen.height;

            UIModule.Data moduleData = new UIModule.Data(Event, canvas);
            m_UIModule = Entry.AddModule<UIModule>(ModuleId, moduleData);
            ClearData();
        }
    }
}
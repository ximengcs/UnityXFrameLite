using System;
using UnityEngine;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;
using XFrame.Modules.Resource;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.Resource;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI模块
    /// </summary>
    [XModule]
    [RequireModule(typeof(PoolModule))]
    public partial class UIModule : SingletonModule<UIModule>
    {
        #region Inner Fields
        private Canvas m_Canvas;
        private Transform m_Root;
        private Dictionary<int, IUI> m_UIMap;
        private Dictionary<Type, IUIFactory> m_Factorys;
        private XLinkList<IUIGroup> m_GroupList;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            InnerCheckCanvas(data);
            if (m_Canvas != null)
            {
                m_Root = m_Canvas.transform;
                m_UIMap = new Dictionary<int, IUI>();
                m_GroupList = new XLinkList<IUIGroup>();
                m_Factorys = new Dictionary<Type, IUIFactory>();
                AddFactory<UI, UI.Factory>();
                AddFactory<MonoUI, MonoUI.Factory>();
            }
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                if (node.Value.IsOpen)
                    node.Value.OnUpdate(escapeTime);
                node = node.Next;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                node.Value.OnDestroy();
                node = node.Next;
            }
            m_GroupList.Dispose();
            m_GroupList = null;
        }
        #endregion

        #region Interface
        /// <summary>
        /// 主UI组
        /// </summary>
        public IUIGroup MainGroup
        {
            get { return InnerGetOrNewGroup(Constant.MAIN_GROUPUI, m_GroupList.Count); }
        }

        #region Open UI
        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, OnUIReady dataHandler = null, bool useNavtive = false, int id = default)
        {
            return Open(uiType, Constant.MAIN_GROUPUI, dataHandler, useNavtive, id);
        }

        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public T Open<T>(OnUIReady dataHandler = null, bool useNavtive = false, int id = default) where T : IUI
        {
            return (T)Open(typeof(T), dataHandler, useNavtive, id);
        }

        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <param name="uiName">UI名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Open(string uiName, OnUIReady dataHandler = null, bool useNavtive = false, int id = default)
        {
            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IUI>();
            Type uiType = typeSys.GetByName(uiName);
            return Open(uiType, dataHandler, useNavtive, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiName">UI名</param>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Open(string uiName, string groupName, OnUIReady dataHandler = null, bool useNavtive = false, int id = default)
        {
            Type uiType = TypeModule.Inst.GetOrNew<IUI>().GetByName(uiName);
            return Open(uiType, groupName, dataHandler, useNavtive, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public T Open<T>(string groupName, OnUIReady dataHandler = null, bool useNavtive = false, int id = default) where T : IUI
        {
            return (T)Open(typeof(T), groupName, dataHandler, useNavtive, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, string groupName, OnUIReady dataHandler = null, bool useNavtive = false, int id = default)
        {
            IUIGroup group = InnerGetOrNewGroup(groupName, m_GroupList.Count);
            return InnerOpenUI(group, uiType, dataHandler, useNavtive, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="group">UI组</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, IUIGroup group, OnUIReady dataHandler = null, bool useNavtive = false, int id = default)
        {
            return InnerOpenUI(group, uiType, dataHandler, useNavtive, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="ui">UI实例</param>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(IUI ui, string groupName, OnUIReady dataHandler = null)
        {
            IUIGroup group = InnerGetOrNewGroup(groupName, m_GroupList.Count);
            return InnerOpenUI(ui, group, dataHandler);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="ui">UI实例</param>
        /// <param name="group">UI组</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(IUI ui, IUIGroup group, OnUIReady dataHandler = null)
        {
            return InnerOpenUI(ui, group, dataHandler);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="group">UI组</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public T Open<T>(IUIGroup group, OnUIReady dataHandler = null, bool useNavtive = false, int id = default)
        {
            return (T)InnerOpenUI(group, typeof(T), dataHandler, useNavtive, id);
        }
        #endregion

        #region Close UI
        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="id">UI Id</param>
        public void Close<T>(int id = default) where T : IUI
        {
            InnerCloseUI(typeof(T), id);
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="id">UI Id</param>
        public void Close(Type uiType, int id = default)
        {
            InnerCloseUI(uiType, id);
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="uiName">UI类型名</param>
        /// <param name="id">UI Id</param>
        public void Close(string uiName, int id = default)
        {
            Type uiType = TypeModule.Inst.GetOrNew<IUI>().GetByName(uiName);
            InnerCloseUI(uiType, id);
        }
        #endregion

        #region Get UI
        /// <summary>
        /// 获取UI
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Get(Type uiType, int id = default)
        {
            return InnerGetUI(uiType, id);
        }

        /// <summary>
        /// 获取UI
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public T Get<T>(int id = default) where T : IUI
        {
            return (T)InnerGetUI(typeof(T), id);
        }

        /// <summary>
        /// 获取UI
        /// </summary>
        /// <param name="uiName">UI类型名</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Get(string uiName, int id = default)
        {
            Type uiType = TypeModule.Inst.GetOrNew<IUI>().GetByName(uiName);
            return InnerGetUI(uiType, id);
        }

        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <param name="ui">待销毁UI</param>
        public void DestroyUI(IUI ui)
        {
            InnerDestroyUI(ui);
        }

        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="id">UI Id</param>
        public void DestroyUI<T>(int id = default) where T : IUI
        {
            DestroyUI(typeof(T), id);
        }

        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <param name="type">UI类型</param>
        /// <param name="id">UI Id</param>
        public void DestroyUI(Type type, int id = default)
        {
            IUI ui = InnerGetUI(type, id);
            if (ui != null)
                DestroyUI(ui);
        }

        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <param name="uiName">UI类型名</param>
        /// <param name="id">UI Id</param>
        public void DestroyUI(string uiName, int id = default)
        {
            Type uiType = TypeModule.Inst.GetOrNew<IUI>().GetByName(uiName);
            DestroyUI(uiType, id);
        }
        #endregion

        /// <summary>
        /// 添加UI创建工厂
        /// </summary>
        /// <typeparam name="UIType">UI类型</typeparam>
        /// <typeparam name="T">工厂类型</typeparam>
        public void AddFactory<UIType, T>() where UIType : IUI where T : IUIFactory
        {
            Type uiType = typeof(UIType);
            if (m_Factorys.ContainsKey(uiType))
                return;
            m_Factorys[uiType] = (T)Activator.CreateInstance(typeof(T));
        }

        /// <summary>
        /// 获取(不存在时创建)UI组
        /// </summary>
        /// <param name="groupName">UI组名称</param>
        /// <param name="layer">UI组层级, 层级大的在层级小的上层显示</param>
        /// <returns>获取到的UI组</returns>
        public IUIGroup GetOrNewGroup(string groupName, int layer = -1)
        {
            if (layer == -1)
                layer = m_GroupList.Count;
            return InnerGetOrNewGroup(groupName, layer);
        }
        #endregion

        #region Inner Implement
        private IUIFactory InnerGetUIFactory(Type uiType)
        {
            if (m_Factorys.TryGetValue(uiType.BaseType, out IUIFactory factory))
                return factory;
            return default;
        }

        private void InnerCloseUI(Type uiType, int id)
        {
            id = InnerEnsureUIId(uiType, id);
            if (m_UIMap.TryGetValue(id, out IUI ui))
                ui.Close();
        }

        private IUI InnerOpenUI(IUIGroup group, Type uiType, OnUIReady dataHandler, bool useNavtive, int id)
        {
            id = InnerEnsureUIId(uiType, id);
            if (!m_UIMap.TryGetValue(id, out IUI ui))
            {
                GameObject prefab;
                string uiPath = $"{Constant.UI_RES_PATH}/{uiType.Name}.prefab";

                if (useNavtive)
                    prefab = Entry.GetModule<NativeResModule>().Load<GameObject>(uiPath);
                else
                    prefab = ResModule.Inst.Load<GameObject>(uiPath);

                if (prefab == null)
                {
                    Log.Error(nameof(UIModule), $"UI res {uiPath} dont exist.");
                    return default;
                }

                GameObject inst = GameObject.Instantiate(prefab);
                IUIFactory factory = InnerGetUIFactory(uiType);
                ui = factory.Create(inst, uiType);
                inst.name = GetInstName(ui);
                ui.OnInit(id, (ui) =>
                {
                    ui.SetData(inst);
                    dataHandler?.Invoke(ui);
                    dataHandler = null;
                });
                m_UIMap[id] = ui;
            }

            return InnerOpenUI(ui, group, dataHandler);
        }

        private IUI InnerOpenUI(IUI ui, IUIGroup group, OnUIReady dataHandler)
        {
            IUIGroup oldGroup = ui.Group;
            if (oldGroup != group)
            {
                oldGroup?.RemoveUI(ui);
                group.AddUI(ui);
                ui.OnGroupChange(group);
            }
            dataHandler?.Invoke(ui);
            ui.Open();
            group.Open();
            return ui;
        }

        private void InnerDestroyUI(IUI ui)
        {
            int id = ui.Id;
            IUIGroup group = ui.Group;
            group?.RemoveUI(ui);
            ui.OnDestroy();
            m_UIMap.Remove(id);
        }

        private void InnerCheckCanvas(object canvas)
        {
            if (canvas != null)
                m_Canvas = (Canvas)canvas;
            if (m_Canvas == null)
                m_Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        private IUI InnerGetUI(Type uiType, int id)
        {
            id = InnerEnsureUIId(uiType, id);
            if (m_UIMap.TryGetValue(id, out IUI ui))
                return ui;
            else
                return default;
        }

        private int InnerEnsureUIId(Type type, int uiId)
        {
            return uiId == default ? type.GetHashCode() : uiId;
        }

        private IUIGroup InnerGetOrNewGroup(string groupName, int layer)
        {
            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                if (node.Value.Name == groupName)
                    return node.Value;
                node = node.Next;
            }

            GameObject groupRoot = new GameObject(groupName, typeof(RectTransform), typeof(CanvasGroup));
            groupRoot.transform.SetParent(m_Root, false);
            IUIGroup group = new UIGroup(groupRoot, groupName, layer);
            group.OnInit();
            m_GroupList.AddLast(group);
            return group;
        }

        internal void SetUIGroupLayer(IUIGroup group, int layer)
        {
            layer = Mathf.Min(layer, m_GroupList.Count);
            layer = Mathf.Max(layer, 0);
            SetLayer(m_Root, group, layer);
        }
        #endregion
    }
}

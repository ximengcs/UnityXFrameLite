using System;
using UnityEngine;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Pools;
using System.Collections.Generic;
using XFrame.Modules.Containers;
using XFrame.Modules.Tasks;
using XFrame.Modules.Event;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Reflection;
using XFrame.Tasks;
using System.Collections;

namespace UnityXFrame.Core.UIElements
{
    /// <summary>
    /// UI模块
    /// </summary>
    [CommonModule]
    [XType(typeof(IUIModule))]
    public partial class UIModule : ModuleBase, IUIModule
    {
        #region Inner Fields
        private Vector2 m_PixelScale;
        private Canvas m_Canvas;
        private Transform m_Root;
        private IEventSystem m_Event;
        private IUIPoolHelper m_Helper;
        private XCollection<IUI> m_UIList;
        private XLinkList<IUIGroup> m_GroupList;
        #endregion

        public Canvas Canvas => m_Canvas;

        public Vector2 PixelScale => m_PixelScale;

        #region Life Fun
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            Data data = userData != null ? (Data)userData : new Data();
            m_Event = data.Event;
            if (m_Event == null)
                m_Event = Domain.GetModule<IEventModule>().NewSys();
            m_Helper = new DefaultUIPoolHelper();
            InnerCheckCanvas(data.Canvas);
            if (m_Canvas != null)
            {
                m_Root = m_Canvas.transform;
                m_UIList = new XCollection<IUI>(Domain);
                m_GroupList = new XLinkList<IUIGroup>();
            }
            Log.Debug("UI", $"UI Canvas pixel scale is {m_PixelScale}");
        }

        public void OnUpdate(float escapeTime)
        {
            foreach (XLinkNode<IUIGroup> node in m_GroupList)
            {
                if (node.Value.IsOpen)
                    node.Value.OnUpdate(escapeTime);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Guid.NewGuid();
            foreach (XLinkNode<IUIGroup> node in m_GroupList)
                node.Value.OnDestroy();
            m_GroupList = null;
        }
        #endregion

        #region Interface
        public XTask PreloadResource(IEnumerable types, int useResModule)
        {
            return m_Helper.PreloadRes(types, useResModule);
        }

        public XTask PreloadResource(Type type, int useResModule)
        {
            return m_Helper.PreloadRes(type, useResModule);
        }

        public async XTask Spwan(IEnumerable types, int useResModule)
        {
            foreach (Type uiType in types)
            {
                if (uiType != null)
                {
                    XLinkList<IPoolObject> list = References.Require<XLinkList<IPoolObject>>();
                    Domain.GetModule<IPoolModule>().GetOrNew(uiType, m_Helper).Spawn(0, 1, useResModule, list);
                    InnerInitSpwanUI(list);
                    list.Clear();
                    References.Release(list);
                    await new XTaskCompleted();
                }
            }
        }

        public async XTask Spwan(Type uiType, int useResModule)
        {
            XLinkList<IPoolObject> list = References.Require<XLinkList<IPoolObject>>();
            Domain.GetModule<IPoolModule>().GetOrNew(uiType, m_Helper).Spawn(0, 1, useResModule, list);
            InnerInitSpwanUI(list);
            list.Clear();
            References.Release(list);
            await new XTaskCompleted();
        }

        private void InnerInitSpwanUI(XLinkList<IPoolObject> list)
        {
            IUIGroup group = InnerGetOrNewGroup(nameof(Spwan), 0);
            group.Close();
            foreach (XLinkNode<IPoolObject> obj in list)
            {
                IUI ui = obj.Value as IUI;
                group.AddUI(ui);
            }
        }

        /// <summary>
        /// 主UI组
        /// </summary>
        public IUIGroup MainGroup
        {
            get { return InnerGetOrNewGroup(Constant.MAIN_GROUPUI, m_GroupList.Count); }
        }

        public IEventSystem Event => m_Event;

        #region Open UI
        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="data">UI数据</param>
        /// <param name="useResModule">使用的资源模块</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default)
        {
            return Open(uiType, Constant.MAIN_GROUPUI, dataHandler, useResModule, id);
        }

        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="data">UI数据</param>
        /// <param name="useResModule">使用的资源模块</param>
        /// <returns>UI实例</returns>
        public T Open<T>(OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default) where T : IUI
        {
            return (T)Open(typeof(T), dataHandler, useResModule, id);
        }

        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <param name="uiName">UI名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useResModule">使用的资源模块</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Open(string uiName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default)
        {
            TypeSystem typeSys = Domain.TypeModule.GetOrNew<IUI>();
            Type uiType = typeSys.GetByName(uiName);
            return Open(uiType, dataHandler, useResModule, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiName">UI名</param>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useResModule">使用的资源模块</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Open(string uiName, string groupName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default)
        {
            Type uiType = Domain.TypeModule.GetOrNew<IUI>().GetByName(uiName);
            return Open(uiType, groupName, dataHandler, useResModule, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useResModule">使用的资源模块</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public T Open<T>(string groupName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default) where T : IUI
        {
            return (T)Open(typeof(T), groupName, dataHandler, useResModule, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useResModule">使用的资源模块</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, string groupName, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default)
        {
            IUIGroup group = InnerGetOrNewGroup(groupName, m_GroupList.Count);
            return InnerOpenUI(group, uiType, dataHandler, useResModule, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="group">UI组</param>
        /// <param name="useResModule">使用的资源模块</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, IUIGroup group, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default)
        {
            return InnerOpenUI(group, uiType, dataHandler, useResModule, id);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="ui">UI实例</param>
        /// <param name="groupName">UI组名</param>
        /// <returns>UI实例</returns>
        public IUI Open(IUI ui, string groupName, OnDataProviderReady dataHandler = null)
        {
            IUIGroup group = InnerGetOrNewGroup(groupName, m_GroupList.Count);
            return InnerOpenUI(ui, group, dataHandler);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="ui">UI实例</param>
        /// <param name="group">UI组</param>
        /// <returns>UI实例</returns>
        public IUI Open(IUI ui, IUIGroup group, OnDataProviderReady dataHandler = null)
        {
            return InnerOpenUI(ui, group, dataHandler);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="group">UI组</param>
        /// <param name="data">UI数据</param>
        /// <param name="useResModule">使用的资源模块</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public T Open<T>(IUIGroup group, OnDataProviderReady dataHandler = null, int useResModule = Constant.COMMON_RES_MODULE, int id = default) where T : IUI
        {
            return (T)InnerOpenUI(group, typeof(T), dataHandler, useResModule, id);
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
            Type uiType = Domain.TypeModule.GetOrNew<IUI>().GetByName(uiName);
            InnerCloseUI(uiType, id);
        }

        public void Close(IUI ui)
        {
            InnerCloseUI(ui);
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
            Type uiType = Domain.TypeModule.GetOrNew<IUI>().GetByName(uiName);
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
            Type uiType = Domain.TypeModule.GetOrNew<IUI>().GetByName(uiName);
            DestroyUI(uiType, id);
        }
        #endregion
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
        private void InnerCloseUI(Type uiType, int id)
        {
            if (uiType == null)
            {
                Log.Debug("UI", $"UI Type is null, close ui failure");
                return;
            }
            IUI ui = m_UIList.Get(uiType, id);
            InnerCloseUI(ui);
        }

        private void InnerCloseUI(IUI ui)
        {
            ui?.Close();
        }

        private IUI InnerOpenUI(IUIGroup group, Type uiType, OnDataProviderReady onReady, int useResModule, int id)
        {
            if (uiType == null)
            {
                Log.Debug("UI", $"UI Type is null, open ui failure");
                return default;
            }
            IUI ui = m_UIList.Get(uiType, id);
            if (ui == null)
            {
                IPool pool = Domain.GetModule<IPoolModule>().GetOrNew(uiType, m_Helper);
                ui = (IUI)pool.Require(default, useResModule);
                ui.OnInit(Domain.GetModule<IContainerModule>(), id, default, onReady);
                onReady = null;
                m_UIList.Add(ui);
            }

            return InnerOpenUI(ui, group, onReady);
        }

        private IUI InnerOpenUI(IUI ui, IUIGroup group, OnDataProviderReady onReady)
        {
            group.AddUI(ui);
            onReady?.Invoke(ui);
            ui.Open();
            return ui;
        }

        private void InnerDestroyUI(IUI ui)
        {
            if (ui == null)
                return;

            IUIGroup group = ui.Group;
            group?.RemoveUI(ui);
            ui.OnDestroy();
            m_UIList.Remove(ui);
            IPool pool = Domain.GetModule<IPoolModule>().GetOrNew(ui.GetType(), m_Helper);
            pool.Release(ui);
        }

        private void InnerCheckCanvas(object canvas)
        {
            if (canvas != null)
                m_Canvas = (Canvas)canvas;
            if (m_Canvas == null)
                m_Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            if (m_Canvas != null)
            {
                RectTransform tf = m_Canvas.GetComponent<RectTransform>();
                m_PixelScale = tf.rect.size / new Vector2(Screen.width, Screen.height);
            }
        }

        private IUI InnerGetUI(Type uiType, int id)
        {
            if (uiType == null)
            {
                Log.Debug("UI", $"UI Type is null, get ui failure");
                return default;
            }
            return m_UIList.Get(uiType, id);
        }

        private IUIGroup InnerGetOrNewGroup(string groupName, int layer)
        {
            foreach (XLinkNode<IUIGroup> node in m_GroupList)
            {
                if (node.Value.Name == groupName)
                    return node.Value;
            }

            GameObject groupRoot = new GameObject(groupName, typeof(RectTransform), typeof(CanvasGroup));
            groupRoot.transform.SetParent(m_Root, false);
            IUIGroup newGroup = new UIGroup(this, groupRoot, groupName, layer);
            newGroup.OnInit();
            m_GroupList.AddLast(newGroup);
            return newGroup;
        }

        internal int SetUIGroupLayer(IUIGroup group, int layer)
        {
            return SetLayer(m_Root, group, layer, InnerGroupLayerChange);
        }

        internal void InnerGroupLayerChange(Transform tf, int index)
        {
            foreach (var uiGroupNode in m_GroupList)
            {
                IUIGroup group = uiGroupNode.Value;
                ICanUpdateLayerValue valueUpdater = group as ICanUpdateLayerValue;
                if (group.Root == tf && valueUpdater != null)
                {
                    valueUpdater.SetLayerValue(index);
                    break;
                }
            }
        }
        #endregion
    }
}

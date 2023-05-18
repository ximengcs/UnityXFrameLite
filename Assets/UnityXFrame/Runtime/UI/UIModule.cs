using System;
using UnityEngine;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;
using System.Collections.Generic;
using XFrame.Modules.Containers;
using UnityEngine.Profiling;

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
        private IPoolHelper m_Helper;
        private XCollection<IUI> m_UIList;
        private XLinkList<IUIGroup> m_GroupList;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Helper = new DefaultUIPoolHelper();
            InnerCheckCanvas(data);
            if (m_Canvas != null)
            {
                m_Root = m_Canvas.transform;
                m_UIList = new XCollection<IUI>();
                m_GroupList = new XLinkList<IUIGroup>();
            }
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            foreach (XLinkNode<IUIGroup> node in m_GroupList)
            {
                if (node.Value.IsOpen)
                    node.Value.OnUpdate(escapeTime);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (XLinkNode<IUIGroup> node in m_GroupList)
                node.Value.OnDestroy();
            m_GroupList = null;
        }
        #endregion

        #region Interface
        public IPoolHelper PoolHelper => m_Helper;

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
        public IUI Open(Type uiType, OnDataProviderReady dataHandler = null, bool useNavtive = false, int id = default)
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
        public T Open<T>(OnDataProviderReady dataHandler = null, bool useNavtive = false, int id = default) where T : IUI
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
        public IUI Open(string uiName, OnDataProviderReady dataHandler = null, bool useNavtive = false, int id = default)
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
        public IUI Open(string uiName, string groupName, OnDataProviderReady dataHandler = null, bool useNavtive = false, int id = default)
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
        public T Open<T>(string groupName, OnDataProviderReady dataHandler = null, bool useNavtive = false, int id = default) where T : IUI
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
        public IUI Open(Type uiType, string groupName, OnDataProviderReady dataHandler = null, bool useNavtive = false, int id = default)
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
        public IUI Open(Type uiType, IUIGroup group, OnDataProviderReady dataHandler = null, bool useNavtive = false, int id = default)
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
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
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
        /// <param name="useNavtive">是否为本地UI</param>
        /// <param name="id">UI Id</param>
        /// <returns>UI实例</returns>
        public T Open<T>(IUIGroup group, OnDataProviderReady dataHandler = null, bool useNavtive = false, int id = default) where T : IUI
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
            IUI ui = m_UIList.Get(uiType, id);
            ui?.Close();
        }

        private IUI InnerOpenUI(IUIGroup group, Type uiType, OnDataProviderReady onReady, bool useNavtive, int id)
        {
            IUI ui = m_UIList.Get(uiType, id);
            if (ui == null)
            {
                IPool pool = PoolModule.Inst.GetOrNew(uiType, m_Helper);
                ui = (IUI)pool.Require(default, useNavtive);
                ui.OnInit(id, default, onReady);
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
            IPool pool = PoolModule.Inst.GetOrNew(ui.GetType(), m_Helper);
            pool.Release(ui);
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
            IUIGroup newGroup = new UIGroup(groupRoot, groupName, layer);
            newGroup.OnInit();
            m_GroupList.AddLast(newGroup);
            return newGroup;
        }

        internal int SetUIGroupLayer(IUIGroup group, int layer)
        {
            return SetLayer(m_Root, group, layer);
        }
        #endregion
    }
}

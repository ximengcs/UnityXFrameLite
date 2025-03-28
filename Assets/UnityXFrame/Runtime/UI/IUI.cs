﻿using UnityEngine;
using XFrame.Modules.Containers;
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.UIElements
{
    /// <summary>
    /// UI
    /// </summary>
    public interface IUI : IUIElement, IContainer, IPoolObject
    {
        /// <summary>
        /// 根节点
        /// </summary>
        RectTransform Root { get; }

        IEventSystem Event { get; }

        /// <summary>
        /// 是否处于打开的状态
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// UI层级, 层级大的在层级小的上层显示
        /// </summary>
        int Layer { get; set; }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="data">数据</param>
        void Open();

        /// <summary>
        /// 关闭UI
        /// </summary>
        void Close();

        /// <summary>
        /// UI所在组
        /// </summary>
        IUIGroup Group { get; internal set; }

        #region Life Fun
        /// <summary>
        /// 打开生命周期，每次UI打开时被调用
        /// </summary>
        /// <param name="data">数据</param>
        protected internal void OnOpen();

        /// <summary>
        /// 关闭生命周期，每次UI关闭时被调用
        /// </summary>
        protected internal void OnClose();
        #endregion
    }
}

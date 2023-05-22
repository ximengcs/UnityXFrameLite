using UnityEngine;
using XFrame.Modules.Containers;
using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI
    /// </summary>
    public interface IUI : IUIElement, IContainer
    {
        /// <summary>
        /// 根节点
        /// </summary>
        RectTransform Root { get; }

        /// <summary>
        /// 是否处于打开的状态
        /// </summary>
        bool IsOpen { get; }

        bool Active { get; set; }

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

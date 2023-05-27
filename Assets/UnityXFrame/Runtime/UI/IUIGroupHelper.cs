using System;
using XFrame.Modules.Event;

namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI组辅助器
    /// </summary>
    public interface IUIGroupHelper
    {
        /// <summary>
        /// 是否处理UI组中的UI类
        /// </summary>
        /// <param name="type">UI类</param>
        /// <returns>true为处理</returns>
        internal bool MatchType(Type type);

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="owner">辅助器所属者</param>
        internal void OnInit(IUIGroup owner);

        /// <summary>
        /// 更新生命周期
        /// </summary>
        internal void OnUpdate();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        internal void OnDestroy(); 

        /// <summary>
        /// UI打开事件
        /// </summary>
        /// <param name="ui">需要打开的UI</param>
        internal void OnUIOpen(IUI ui);

        /// <summary>
        /// UI关闭事件
        /// </summary>
        /// <param name="ui">需要关闭的UI</param>
        internal void OnUIClose(IUI ui);

        /// <summary>
        /// UI更新事件
        /// </summary>
        /// <param name="ui">需要更新的UI</param>
        internal void OnUIUpdate(IUI ui, float elapseTime);

        /// <summary>
        /// UI销毁事件
        /// </summary>
        /// <param name="ui">需要销毁的UI</param>
        internal void OnUIDestroy(IUI ui);
    }
}

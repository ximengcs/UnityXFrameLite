using System;
using UnityXFrame.Core.UIs;

namespace UnityXFrameLib.UI
{
    /// <summary>
    /// UI组辅助器效果
    /// </summary>
    public interface IUIGroupHelperEffect
    {
        /// <summary>
        /// 播放效果
        /// </summary>
        /// <param name="ui">UI</param>
        /// <param name="onComplete">回调</param>
        bool Do(IUI ui, Action onComplete);

        /// <summary>
        /// 停止并销毁效果
        /// </summary>
        /// <param name="ui">UI</param>
        bool Kill(IUI ui);

        internal void OnUpdate();
    }
}
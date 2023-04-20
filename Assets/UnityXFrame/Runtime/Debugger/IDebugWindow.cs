using System;

namespace UnityXFrame.Core.Diagnotics
{
    /// <summary>
    /// 调试窗口接口
    /// </summary>
    public interface IDebugWindow : IDisposable
    {
        /// <summary>
        /// 唤醒生命周期函数
        /// </summary>
        void OnAwake();

        /// <summary>
        /// 绘制生命周期函数
        /// </summary>
        void OnDraw();
    }
}
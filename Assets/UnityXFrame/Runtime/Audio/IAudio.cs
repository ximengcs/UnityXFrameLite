using System;
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.Audios
{
    /// <summary>
    /// 音源
    /// </summary>
    public interface IAudio : IPoolObject
    {
        /// <summary>
        /// 设置是否自动释放，当置为true且播放模式为非循环时在播放结束时会自动释放
        /// </summary>
        bool AutoRelease { get; set; }

        /// <summary>
        /// 当被释放时或不可用时为true
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// 音量(0-1)
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// 音组
        /// </summary>
        IAudioGroup Group { get; }

        /// <summary>
        /// 播放一次(从头播放)，当 <see cref="AutoRelease"/> 置为true时，在播放结束时自动释放，
        /// 当置为false时需要调用 <see cref="Release"/> 释放对象
        /// </summary>
        /// <param name="callback">完成回调</param>
        void Play(Action callback = null);

        /// <summary>
        /// 循环播放(从头播放)，循环播放不会自动释放
        /// </summary>
        void PlayLoop();

        /// <summary>
        /// 停止播放，当 <see cref="AutoRelease"/> 置为true时，调用此方法会释放对象
        /// </summary>
        void Stop();

        /// <summary>
        /// 暂停播放
        /// </summary>
        void Pause();

        /// <summary>
        /// 继续播放，若当前为暂停的状态则继续播放，否则从头播放
        /// </summary>
        void Continue();

        /// <summary>
        /// 释放对象
        /// </summary>
        void Release();
    }
}

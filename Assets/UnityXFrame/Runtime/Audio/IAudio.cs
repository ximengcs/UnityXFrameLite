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
        /// 音量(0-1)
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// 音组
        /// </summary>
        IAudioGroup Group { get; }

        /// <summary>
        /// 播放一次
        /// </summary>
        /// <param name="callback">完成回调</param>
        void Play(Action callback = null);

        /// <summary>
        /// 循环播放
        /// </summary>
        void PlayLoop();

        /// <summary>
        /// 停止播放
        /// </summary>
        void Stop();
    }
}

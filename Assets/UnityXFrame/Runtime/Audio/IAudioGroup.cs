
namespace UnityXFrame.Core.Audios
{
    /// <summary>
    /// 音组
    /// </summary>
    public interface IAudioGroup
    {
        /// <summary>
        /// 组音量
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// 播放
        /// </summary>
        void Play();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 添加一个音源
        /// </summary>
        /// <param name="audio">音源</param>
        void Add(IAudio audio);
    }
}

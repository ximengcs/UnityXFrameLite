
using System;
using XFrame.Core;
using XFrame.Tasks;

namespace UnityXFrame.Core.Audios
{
    public interface IAudioModule : IModule
    {
        float Volume { get; set; }

        IAudioGroup MainGroup { get; }

        IAudioGroup GetOrNewGroup(string groupName);

        void RemoveGroup(string groupName);

        IAudio Play(string name, Action callback = null);

        IAudio Play(string name, bool autoRelease, Action callback = null);

        IAudio Play(string name, string groupName, Action callback = null);

        IAudio Play(string name, string groupName, bool autoRelease, Action callback = null);

        IAudio PlayLoop(string name, bool autoRelease = false);

        IAudio PlayLoop(string name, string groupName, bool autoRelease = false);

        XTask<IAudio> PlayAsync(string name, Action callback = null);

        XTask<IAudio> PlayAsync(string name, bool autoRelease, Action callback = null);

        XTask<IAudio> PlayAsync(string name, string groupName, Action callback = null);

        XTask<IAudio> PlayAsync(string name, string groupName, bool autoRelease, Action callback = null);

        XTask<IAudio> PlayLoopAsync(string name, bool autoRelease = false);

        XTask<IAudio> PlayLoopAsync(string name, string groupName, bool autoRelease = false);
    }
}

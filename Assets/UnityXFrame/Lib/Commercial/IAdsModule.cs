using System;
using XFrame.Core;

namespace UnityXFrameLib.Commercial
{
    public interface IAdsModule : IModule
    {
        IAdView Open(AdsData data);
        void Close(AdsConfig config);
        void Close(int adType, int entityId = default);
        void Register(AdsConfig config);
        bool GetConfig(int type, int viewId, out AdsConfig config);
        void WaitInit(Action handler);
    }
}

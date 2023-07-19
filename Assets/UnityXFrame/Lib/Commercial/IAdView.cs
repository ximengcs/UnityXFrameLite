using XFrame.Collections;
using XFrame.Modules.Event;

namespace UnityXFrameLib.Commercial
{
    public interface IAdView : IXItem
    {
        IEventSystem Event { get; }
        void Open();
        void Close();
        void OnInit(AdsData data, AdsConfig config);
    }
}

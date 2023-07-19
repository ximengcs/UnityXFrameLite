
namespace UnityXFrameLib.Commercial
{
    public struct AdsConfig
    {
        public AdType Type;
        public string AdUnitId;
        public int ViewId;
        public object UserData;

        public AdsConfig(AdType type, string adUnitId, int viewId = 0, object userData = null)
        {
            Type = type;
            AdUnitId = adUnitId;
            ViewId = viewId;
            UserData = userData;
        }
    }
}

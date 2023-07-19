
namespace UnityXFrameLib.Commercial
{
    public struct AdsData
    {
        public AdType Type;
        public int ViewId;
        public int EntityId;
        public object UserData;

        public AdsData(AdType type, int viewId, int entityId = 0, object userData = null)
        {
            Type = type;
            ViewId = viewId;
            EntityId = entityId;
            UserData = userData;
        }

        public AdsData(AdType type, int entityId = 0, object userData = null)
        {
            Type = type;
            ViewId = 0;
            EntityId = entityId;
            UserData = userData;
        }
    }
}

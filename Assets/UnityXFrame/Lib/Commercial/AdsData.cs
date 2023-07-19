
namespace UnityXFrameLib.Commercial
{
    public struct AdsData
    {
        public int Type;
        public int ViewId;
        public int EntityId;
        public object UserData;

        public AdsData(int type, int viewId, int entityId = 0, object userData = null)
        {
            Type = type;
            ViewId = viewId;
            EntityId = entityId;
            UserData = userData;
        }

        public AdsData(int type, int entityId = 0, object userData = null)
        {
            Type = type;
            ViewId = 0;
            EntityId = entityId;
            UserData = userData;
        }

        public AdsData(int type, object userData = null)
        {
            Type = type;
            ViewId = 0;
            EntityId = 0;
            UserData = userData;
        }
    }
}

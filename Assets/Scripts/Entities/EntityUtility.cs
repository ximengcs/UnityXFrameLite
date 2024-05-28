using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public static class EntityUtility
    {
        public static string Name(this IEntity entity)
        {
            IMailBox mailBox = entity.GetCom<IMailBox>(0, false);
            return $"[{entity.GetType().Name}] {entity.Id} {(mailBox != null ? $"-> [{mailBox.GetType().Name}] {mailBox.Id}" : string.Empty)}";
        }

        public static UnityEngine.Vector3 ToUnityPos(this System.Numerics.Vector3 pos)
        {
            return new UnityEngine.Vector3(pos.X, pos.Y, pos.Z);
        }

        public static System.Numerics.Vector3 ToStandardPos(this UnityEngine.Vector3 pos)
        {
            return new System.Numerics.Vector3(pos.x, pos.y, pos.z);
        }
    }
}

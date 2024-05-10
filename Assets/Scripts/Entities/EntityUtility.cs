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
    }
}

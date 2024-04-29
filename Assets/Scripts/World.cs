
using Game.Network;
using XFrame.Core;

namespace Game
{
    public class World
    {
        private static NetWorkModule s_Net;

        public static NetWorkModule Net => s_Net ?? Entry.GetModule<NetWorkModule>();
    }
}

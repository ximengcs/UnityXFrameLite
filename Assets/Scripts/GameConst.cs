
using XFrame.Core.Threads;

namespace Assets.Scripts
{
    public static class GameConst
    {
        private static int s_FIBER_ID = 1000;
        public static int FIBER_ID => s_FIBER_ID++;

        private const int NET_FIBER = 1;

        private static Fiber s_NetFiber;
        public static Fiber NetFiber => s_NetFiber;

        public static void Initialize()
        {
            s_NetFiber = Global.Fiber.GetOrNew(NET_FIBER);
            s_NetFiber.StartThread(1);
            Global.Net.SetFiber(s_NetFiber);
        }
    }
}

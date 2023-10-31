
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace UnityXFrameLib.Effects
{
    public partial class ParticleEffect
    {
        public class DestroyEvent : XEvent
        {
            private static int s_EventId;

            public static int EventId
            {
                get
                {
                    if (s_EventId == default)
                        s_EventId = typeof(DestroyEvent).GetHashCode();
                    return s_EventId;
                }
            }

            public ParticleEffect Inst { get; private set; }

            private DestroyEvent() { }

            public static DestroyEvent Create(ParticleEffect inst)
            {
                DestroyEvent e = References.Require<DestroyEvent>();
                e.Id = EventId;
                e.Inst = inst;
                return e;
            }

            protected override void OnReleaseFromPool()
            {
                base.OnReleaseFromPool();
                Inst = default;
            }
        }
    }
}

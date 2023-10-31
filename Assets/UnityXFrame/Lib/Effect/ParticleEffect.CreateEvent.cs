using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace UnityXFrameLib.Effects
{
    public partial class ParticleEffect
    {
        public class CreateEvent : XEvent
        {
            private static int s_EventId;

            public static int EventId
            {
                get
                {
                    if (s_EventId == default)
                        s_EventId = typeof(CreateEvent).GetHashCode();
                    return s_EventId;
                }
            }

            public ParticleEffect Inst { get; private set; }

            private CreateEvent() { }

            public static CreateEvent Create(ParticleEffect inst)
            {
                CreateEvent e = References.Require<CreateEvent>();
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

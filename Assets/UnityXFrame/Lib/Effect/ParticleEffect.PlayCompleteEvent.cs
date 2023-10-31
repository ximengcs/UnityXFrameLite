using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace UnityXFrameLib.Effects
{
    public partial class ParticleEffect
    {
        public class PlayCompleteEvent : XEvent
        {
            private static int s_EventId;

            public static int EventId
            {
                get
                {
                    if (s_EventId == default)
                        s_EventId = typeof(PlayCompleteEvent).GetHashCode();
                    return s_EventId;
                }
            }

            public ParticleEffect Inst { get; private set; }

            private PlayCompleteEvent() { }

            public static PlayCompleteEvent Create(ParticleEffect inst)
            {
                PlayCompleteEvent e = References.Require<PlayCompleteEvent>();
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

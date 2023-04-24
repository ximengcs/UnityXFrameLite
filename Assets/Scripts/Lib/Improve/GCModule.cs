using XFrame.Core;
using XFrame.Modules.Tasks;

namespace UnityXFrameLib.Improve
{
    public class GCModule : SingletonModule<GCModule>
    {
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            InnerToMunal();
        }

        public GCTask Request()
        {
            return TaskModule.Inst.GetOrNew<GCTask>();
        }

        private void InnerToMunal()
        {
#if !UNITY_EDITOR
            if (GarbageCollector.GCMode != GarbageCollector.Mode.Manual)
                GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
#endif
        }
    }
}

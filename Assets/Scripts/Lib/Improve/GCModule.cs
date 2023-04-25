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
            if (UnityEngine.Scripting.GarbageCollector.GCMode != UnityEngine.Scripting.GarbageCollector.Mode.Manual)
                UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Manual;
#endif
        }
    }
}

using XFrame.Collections;
using XFrame.Core;

namespace UnityXFrameLib.Improve
{
    [XType(typeof(IGCModule))]
    public class GCModule : ModuleBase, IGCModule
    {
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            InnerToMunal();
        }

        public GCTask Request()
        {
            return XModule.Task.GetOrNew<GCTask>();
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

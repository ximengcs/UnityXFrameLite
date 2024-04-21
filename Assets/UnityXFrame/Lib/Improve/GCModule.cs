using XFrame.Core;
using UnityXFrame.Core;
using XFrame.Collections;

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
            return new GCTask();
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

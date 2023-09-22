using XFrame.Core;

namespace UnityXFrameLib.Improve
{
    public class GCModule : ModuleBase, IModule
    {
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            InnerToMunal();
        }

        public GCTask Request()
        {
            return Module.Task.GetOrNew<GCTask>();
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

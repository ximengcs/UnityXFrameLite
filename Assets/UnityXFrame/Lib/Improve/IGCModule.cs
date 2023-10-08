using XFrame.Core;

namespace UnityXFrameLib.Improve
{
    public interface IGCModule : IModule
    {
        GCTask Request();
    }
}

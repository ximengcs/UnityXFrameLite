using XFrame.Core;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public interface ISpriteAtlasModule : IResRedirectHelper, IModule
    {
        void AddEntry(string entryText);
    }
}

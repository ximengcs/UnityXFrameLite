
using System;
using XFrame.Core;

namespace UnityXFrame.Core
{
    public class EndOfFrameHandler : IModuleHandler
    {
        public Type Target => typeof(IEndOfFrame);

        public void Handle(IModule module, object data)
        {
            IEndOfFrame endOfFrame = (IEndOfFrame)module;
            endOfFrame.OnEndOfFrame();
        }
    }
}

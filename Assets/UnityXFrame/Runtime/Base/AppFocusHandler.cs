using System;
using XFrame.Core;

namespace UnityXFrame.Core
{
    public class AppFocusHandler : IModuleHandler
    {
        public Type Target => typeof(IAppFocus);

        public void Handle(IModule module, object data)
        {
            IAppFocus entity = module as IAppFocus;
            entity.OnFocus();
        }
    }
}

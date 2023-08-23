using System;
using XFrame.Core;

namespace UnityXFrame.Core
{
    public class Saver : IModuleHandler
    {
        public Type Target => typeof(ISaveable);

        public void Handle(IModule module, object data)
        {
            ISaveable saver = module as ISaveable;
            saver.Save();
        }
    }
}

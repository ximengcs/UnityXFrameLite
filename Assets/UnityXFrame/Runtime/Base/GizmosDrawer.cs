
using System;
using XFrame.Core;

namespace UnityXFrame.Core
{
    public class GizmosDrawer : IModuleHandler
    {
        public Type Target => typeof(IGizmos);

        public void Handle(IModule module, object data)
        {
            IGizmos gizmos = (IGizmos)module;
            gizmos.OnGizmos();
        }
    }
}

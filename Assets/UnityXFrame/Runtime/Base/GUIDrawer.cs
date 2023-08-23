
using System;
using XFrame.Core;

namespace UnityXFrame.Core
{
    public class GUIDrawer : IModuleHandler
    {
        public Type Target => typeof(IGUI);

        public void Handle(IModule module, object data)
        {
            IGUI gui = module as IGUI;
            gui.OnGUI();
        }
    }
}

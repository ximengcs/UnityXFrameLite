using XFrame.Core;
using XFrame.Modules.Resource;
using UnityXFrame.Core.Resource;

namespace UnityXFrame.Core
{
    [XModule]
    internal class MultiModuleAdder : ModuleBase
    {
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            Entry.AddModule<ResModule>(Constant.LOCAL_RES_MODULE).SetHelper(typeof(ResourcesHelper));
        }
    }
}

using XFrame.Core;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    /// <summary>
    /// 本地资源加载 (Resources)
    /// </summary>
    [XModule]
    public partial class NativeResModule : ResModule
    {
        public new static NativeResModule Inst { get; private set; }

        protected override void OnInit(object data)
        {
            Id = 1;
            Inst = this;
            InnerSetHelper(typeof(ResourcesHelper));
        }
    }
}

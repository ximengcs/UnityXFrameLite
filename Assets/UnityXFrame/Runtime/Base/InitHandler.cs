using UnityXFrame.Core.Resource;
using XFrame.Core;
using XFrame.Modules.Resource;
using XFrame.Modules.Tasks;

namespace UnityXFrame.Core
{
    public class InitHandler : IInitHandler
    {
        public void EnterHandle()
        {
            
        }

        public ITask BeforeHandle()
        {
            return Module.Task.GetOrNew<EmptyTask>();
        }

        public ITask AfterHandle()
        {
            InnerConfigLog();
            InnerAddExtModule();
            return Module.Task.GetOrNew<EmptyTask>();
        }

        private void InnerConfigLog()
        {
            Diagnotics.Logger logger = Module.Log.GetLogger<Diagnotics.Logger>();
            foreach (DebugColor colorData in Init.Inst.Data.LogMark)
            {
                if (colorData.Value)
                    logger.Register(colorData.Key, colorData.Color);
            }
        }

        private void InnerAddExtModule()
        {
            Entry.AddModule<ResModule>(Constant.LOCAL_RES_MODULE)
                 .SetHelper(typeof(ResourcesHelper));
        }
    }
}

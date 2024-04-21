using UnityXFrame.Core.Resource;
using XFrame.Core;
using XFrame.Modules.Resource;
using XFrame.Tasks;

namespace UnityXFrame.Core
{
    public class InitHandler : IInitHandler
    {
        public void EnterHandle()
        {

        }

        public async XTask BeforeHandle()
        {
            await new XTaskCompleted();
        }

        public async XTask AfterHandle()
        {
            InnerConfigLog();
            InnerAddExtModule();
            await new XTaskCompleted();
        }

        private void InnerConfigLog()
        {
            Diagnotics.Logger logger = Global.Log.GetLogger<Diagnotics.Logger>();
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

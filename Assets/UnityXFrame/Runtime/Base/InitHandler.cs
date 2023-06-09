﻿using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core
{
    public class InitHandler : IInitHandler
    {
        public void EnterHandle()
        {
            
        }

        public ITask BeforeHandle()
        {
            return TaskModule.Inst.GetOrNew<EmptyTask>();
        }

        public ITask AfterHandle()
        {
            InnerConfigLog();
            return TaskModule.Inst.GetOrNew<EmptyTask>();
        }

        private void InnerConfigLog()
        {
            Diagnotics.Logger logger = LogModule.Inst.GetLogger<Diagnotics.Logger>();
            foreach (DebugColor colorData in Init.Inst.Data.LogMark)
            {
                if (colorData.Value)
                    logger.Register(colorData.Key, colorData.Color);
            }
        }
    }
}

using Game.Test;
using System.Reflection;
using UnityEngine;
using UnityXFrame.Core;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.UIElements;
using XFrame.Core;
using XFrame.Modules.Procedure;
using XFrame.Modules.Resource;

namespace Game.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
#if CONSOLE
            Entry.AddModule<Debugger>();
#endif
            Application.targetFrameRate = 60;
            ChangeState<EnterGameProcedure>();
            //ChangeState<CheckResUpdateProcedure>();
        }
    }
}

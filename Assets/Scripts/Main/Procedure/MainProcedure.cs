using Game.Test;
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
            Entry.AddModule<Debuger>();
            Application.targetFrameRate = 60;
            ChangeState<EnterGameProcedure>();
            //ChangeState<CheckResUpdateProcedure>();

            GameObject prefab = Entry.GetModule<ResModule>(Constant.LOCAL_RES_MODULE).Load<GameObject>("Data/Prefab/TestSceneUI.prefab");
            GameObject inst = GameObject.Instantiate(prefab);
            Canvas canvas = inst.GetComponentInChildren<Canvas>();
            UIModule module = Entry.AddModule<UIModule>(1, canvas);
            module.GetOrNewGroup("TestScene");
            module.Open<SettingUI>("Test", null, Constant.LOCAL_RES_MODULE);
        }
    }
}

using UnityXFrame.Core.UIs;

namespace Game.Test
{
    public partial class LoadingUI : UI
    {
        protected override void OnInit()
        {
            base.OnInit();
            UICommonCom com = AddCom<UICommonCom>();
            com.Add("Progress");
            AddCom<TestCom>();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }
    }
}

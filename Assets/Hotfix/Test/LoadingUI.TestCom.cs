using UnityEngine.UI;
using UnityXFrame.Core.UIs;

namespace Game.Test
{
    public partial class LoadingUI
    {
        public class TestCom : UICom
        {
            private Slider m_Slider;

            protected override void OnInit()
            {
                base.OnInit();
                m_Slider = GetCom<UICommonCom>().Get<Slider>("Progress");
            }

            protected override void OnOpen()
            {
                base.OnOpen();
                float pro = Owner.GetData<float>();
                m_Slider.value = pro;
            }

            protected override void OnClose()
            {
                base.OnClose();
                m_Slider.value = 0;
            }
        }
    }
}

using XFrame.Core;
using UnityXFrame.Core.UIs;

namespace UnityXFrameLib.UI
{
    /// <summary>
    /// 附带UI打开关闭效果的组辅助器
    /// </summary>
    public class UIGroupHelperInEffect : UIGroupHelperBase
    {
        protected IUIGroupHelperEffect m_OpenEffect;
        protected IUIGroupHelperEffect m_CloseEffect;

        protected override void OnInit()
        {
            base.OnInit();
        }

        public void SetEffect(IUIGroupHelperEffect open, IUIGroupHelperEffect close)
        {
            m_OpenEffect = open;
            m_CloseEffect = close;
        }
    }
}

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

        public UIGroupHelperInEffect(IUIGroupHelperEffect openEffect, IUIGroupHelperEffect closeEffect)
        {
            m_OpenEffect = openEffect;
            m_CloseEffect = closeEffect;
        }
    }
}

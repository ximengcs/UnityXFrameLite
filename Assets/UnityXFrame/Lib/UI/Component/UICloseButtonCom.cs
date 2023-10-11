using UnityEngine.UI;
using UnityXFrame.Core.UIElements;

namespace UnityXFrameLib.UIElements
{
    public class UICloseButtonCom : UICom
    {
        private string m_ComName = "CloseBtn";
        private Button m_CloseBtn;

        protected override void OnInit()
        {
            base.OnInit();
            m_CloseBtn = m_UIFinder.GetUI<Button>(m_ComName);
            m_CloseBtn.onClick.AddListener(InnerHandlerClose);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_CloseBtn.onClick.RemoveListener(InnerHandlerClose);
        }

        private void InnerHandlerClose()
        {
            m_UI.Close();
        }
    }
}

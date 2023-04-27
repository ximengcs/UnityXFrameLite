using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityXFrame.Core.UIs;
using UnityXFrameLib.Localize;
using XFrame.Modules.Event;
using XFrame.Modules.Local;

namespace Game.Test
{
    public class DialogUI : UI
    {
        private TextMeshProUGUI m_Title;

        protected override void OnInit()
        {
            base.OnInit();
            m_Transform.anchoredPosition += GetData<Vector2>();
            m_Transform.Find("BackGround").GetComponent<Image>().color = GetData<Color>();

            m_Title = m_Transform.Find("Text").GetComponent<TextMeshProUGUI>();
            LocalizeExt.RegisterLocalText(m_Title, InnerLangChange);
        }

        private void InnerLangChange(TextMeshProUGUI textCom)
        {
            textCom.text = LocalizeModule.Inst.GetValue(1);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityXFrame.Core.UIs;
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
            LocalizeModule.Inst.Event.Listen(LanguageChangeEvent.EventId, InnerLangChange);
            LocalizeModule.Inst.Lang = Language.English;
        }

        private void InnerLangChange(XEvent e)
        {
            m_Title.text = LocalizeModule.Inst.GetValue(1);
        }
    }
}

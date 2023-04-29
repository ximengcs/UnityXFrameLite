using TMPro;
using System;
using XFrame.Modules.Local;
using UnityXFrame.Core.UIs;
using UnityXFrameLib.Localize;
using System.Collections.Generic;

namespace Game.Test
{
    public class SettingUI : UI
    {
        private TextMeshProUGUI m_Title;
        private TestDropDown m_Dropdown;
        private List<string> m_Options;
        private List<TMP_FontAsset> m_Fonts;
        private List<Language> m_Languages;

        protected override void OnInit()
        {
            base.OnInit();
            InnerInitLanguage();
            m_Dropdown = m_Transform.Find("Language").GetComponent<TestDropDown>();
            m_Title = m_Transform.Find("Title").GetComponent<TextMeshProUGUI>();
            m_Dropdown.SetFonts(m_Fonts);
            m_Dropdown.AddOptions(m_Options);
            m_Dropdown.onValueChanged.AddListener(InnerChangeHandler);
            m_Dropdown.value = m_Languages.IndexOf(LocalizeModule.Inst.Lang);
            LocalizeExt.RegisterLocalText(m_Title, InnerSetTitle);
        }

        private void InnerSetTitle(TextMeshProUGUI textCom)
        {
            textCom.text = LocalizeModule.Inst.GetValue(5);
            m_Dropdown.captionText.font = textCom.font;
        }

        private void InnerInitLanguage()
        {
            m_Languages = new List<Language>();
            m_Fonts = new List<TMP_FontAsset>();
            m_Options = new List<string>();
            Array list = Enum.GetValues(typeof(Language));
            foreach (Language language in list)
            {
                TMP_FontAsset asset = LocalizeExt.LoadFont(language);
                if (asset != null)
                {
                    m_Languages.Add(language);
                    m_Fonts.Add(asset);
                    m_Options.Add(LocalizeModule.Inst.GetValue(language, 4));
                }
            }
        }

        private void InnerChangeHandler(int select)
        {
            SettingData.Inst.Lang.Value = m_Languages[select];
        }
    }
}

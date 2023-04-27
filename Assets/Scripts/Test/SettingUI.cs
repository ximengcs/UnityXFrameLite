using TMPro;
using XFrame.Modules.Local;
using UnityXFrame.Core.UIs;
using UnityXFrameLib.Localize;
using System.Collections.Generic;

namespace Game.Test
{
    public class SettingUI : UI
    {
        private TestDropDown m_Dropdown;
        private List<string> m_Options;
        private List<Language> m_Languages;

        protected override void OnInit()
        {
            base.OnInit();
            InnerInitLanguage();
            m_Dropdown = m_Transform.Find("Language").GetComponent<TestDropDown>();
            m_Dropdown.SetFonts(InnerCollectFonts());
            m_Dropdown.AddOptions(m_Options);
            m_Dropdown.onValueChanged.AddListener(InnerChangeHandler);
        }

        private List<TMP_FontAsset> InnerCollectFonts()
        {
            List<TMP_FontAsset> list = new List<TMP_FontAsset>(m_Languages.Count);
            foreach (Language language in m_Languages)
                list.Add(LocalizeExt.LoadFont(language));
            return list;
        }

        private void InnerInitLanguage()
        {
            m_Languages = new List<Language>()
            {
                Language.English,
                Language.ChineseSimplified,
                Language.Japanese
            };
            m_Options = new List<string>(LocalizeModule.Inst.GetLine(4));
        }

        private void InnerChangeHandler(int select)
        {
            LocalizeModule.Inst.Lang = m_Languages[select];
        }
    }
}

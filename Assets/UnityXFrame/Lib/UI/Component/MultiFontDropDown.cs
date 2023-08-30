using TMPro;
using UnityEngine;
using UnityXFrame.Core;
using System.Collections.Generic;

namespace UnityXFrameLib.UIElements
{
    [DefaultExecutionOrder(Constant.EXECORDER_AFTER)]
    public class MultiFontDropDown : TMP_Dropdown
    {
        private int m_Index;
        private List<TMP_FontAsset> m_Fonts;

        protected override void Start()
        {
            base.Start();
            onValueChanged.AddListener(InnerSelectChange);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onValueChanged.RemoveListener(InnerSelectChange);
        }

        public void SetFonts(List<TMP_FontAsset> fonts)
        {
            m_Fonts = fonts;
        }

        protected override GameObject CreateDropdownList(GameObject template)
        {
            m_Index = 0;
            return base.CreateDropdownList(template);
        }

        private void InnerSelectChange(int select)
        {
            captionText.font = m_Fonts[select];
        }

        protected override DropdownItem CreateItem(DropdownItem itemTemplate)
        {
            DropdownItem item = base.CreateItem(itemTemplate);
            item.text.font = m_Fonts[m_Index++];
            return item;
        }
    }
}

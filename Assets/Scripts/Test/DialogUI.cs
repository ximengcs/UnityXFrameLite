using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityXFrame.Core.UIs;
using UnityXFrameLib.Localize;
using UnityXFrameLib.UI;
using XFrame.Modules.Event;
using XFrame.Modules.Local;

namespace Game.Test
{
    [AutoLoadUI(true)]
    [AutoSpwanUI(true)]
    public class DialogUI : MonoUI
    {
        public TextMeshProUGUI m_Title;
        public Image m_BackGround;

        protected override void OnInit()
        {
            base.OnInit();
            AddCom<DragUICom>((db) => DragUICom.SetTarget(db, "Rect", "BackGround"));
            m_Transform.anchoredPosition += GetData<Vector2>();
            m_BackGround.color = GetData<Color>();
            LocalizeExt.RegisterLocalText(m_Title, InnerLangChange);
        }

        private void InnerLangChange(TextMeshProUGUI textCom)
        {
            textCom.text = LocalizeModule.Inst.GetValue(1);
        }
    }
}

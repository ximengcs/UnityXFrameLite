using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityXFrameLib.UIElements;
using UnityXFrame.Core.UIElements;
using UnityXFrameLib.Localize;
using UnityXFrame.Core;

namespace Game.Test
{
    [AutoLoadUI(Constant.LOCAL_RES_MODULE)]
    [AutoSpwanUI(Constant.LOCAL_RES_MODULE)]
    public class DialogUI : MonoUI
    {
        public TextMeshProUGUI m_Title;
        public Image m_BackGround;

        protected override void OnInit()
        {
            base.OnInit();
            AddCom<UICloseButtonCom>();
            AddCom<DragUICom>((db) => DragUICom.SetTarget(db, "Rect", "BackGround"));
            m_Transform.anchoredPosition += GetData<Vector2>();

            Color orginColor = m_BackGround.color;
            m_BackGround.color = GetData<Color>();
            if (m_BackGround.color != Color.black)
                m_BackGround.color = orginColor;

            LocalizeExt.Register(m_Title, InnerLangChange);
        }

        private void InnerLangChange(TextMeshProUGUI textCom)
        {
            textCom.SetLocal(1);
        }
    }
}

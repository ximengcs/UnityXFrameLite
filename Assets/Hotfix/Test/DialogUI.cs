using UnityEngine;
using UnityEngine.UI;
using UnityXFrame.Core.UIs;

namespace Game.Test
{
    public class DialogUI : UI
    {
        protected override void OnInit()
        {
            base.OnInit();
            m_Transform.anchoredPosition += GetData<Vector2>();
            m_Transform.Find("BackGround").GetComponent<Image>().color = GetData<Color>();
        }
    }
}

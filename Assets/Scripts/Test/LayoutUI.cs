using UnityEngine;
using UnityXFrame.Core;
using UnityXFrame.Core.UIElements;
using UnityXFrameLib.UIElements;

namespace Test
{
    [AutoLoadUI(Constant.LOCAL_RES_MODULE)]
    [AutoSpwanUI(Constant.LOCAL_RES_MODULE)]
    public class LayoutUI : UI
    {
        private Animator m_Anim;

        public CanvasUICom Left { get; private set; }
        public CanvasUICom Right { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();
            m_Anim = m_Root.GetComponent<Animator>();
            Left = AddCom<CanvasUICom>((db) => db.SetData(nameof(Left)));
            Right = AddCom<CanvasUICom>((db) => db.SetData(nameof(Right)));

            Left.MainGroup.AddHelper<MultiUIGroupHelper>()
                .SetEffect(new AnimatorStateEffect("Open"), new AnimatorStateEffect("Close"));
            Right.MainGroup.AddHelper<MultiUIGroupHelper>()
                .SetEffect(new AnimatorStateEffect("Open"), new AnimatorStateEffect("Close"));
        }

        public void OpenLeft()
        {
            m_Anim.SetTrigger("OpenLeft");
        }

        public void CloseLeft()
        {
            m_Anim.SetTrigger("CloseLeft");
        }

        public void OpenRight()
        {
            m_Anim.SetTrigger("OpenRight");
        }

        public void CloseRight()
        {
            m_Anim.SetTrigger("CloseRight");
        }

        public void OpenTop()
        {
        }

        public void CloseTop()
        {
        }
    }
}
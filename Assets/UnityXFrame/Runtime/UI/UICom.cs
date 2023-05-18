using XFrame.Modules.Containers;

namespace UnityXFrame.Core.UIs
{
    public abstract class UICom : ShareCom
    {
        protected UICommonCom m_CommonCom;

        protected override void OnInit()
        {
            base.OnInit();
            m_CommonCom = GetOrAddCom<UICommonCom>();
        }

        protected internal virtual void OnOpen() { }

        protected internal virtual void OnClose() { }
    }
}

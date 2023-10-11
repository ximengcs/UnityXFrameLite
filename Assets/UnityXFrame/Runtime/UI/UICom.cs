using XFrame.Modules.Containers;

namespace UnityXFrame.Core.UIElements
{
    public abstract class UICom : ShareCom
    {
        protected UIFinder m_UIFinder;
        protected IUI m_UI;

        protected override void OnInit()
        {
            base.OnInit();
            m_UI = Master as IUI;
            m_UIFinder = GetOrAddCom<UIFinder>();
        }

        protected internal virtual void OnOpen() { }

        protected internal virtual void OnClose() { }
    }
}

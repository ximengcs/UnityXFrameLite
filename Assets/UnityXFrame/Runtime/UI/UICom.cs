using XFrame.Modules.Containers;

namespace UnityXFrame.Core.UIs
{
    public abstract class UICom : ShareCom
    {
        protected UIFinder m_UIFinder;

        protected override void OnInit()
        {
            base.OnInit();
            m_UIFinder = GetOrAddCom<UIFinder>();
        }

        protected internal virtual void OnOpen() { }

        protected internal virtual void OnClose() { }
    }
}


namespace UnityXFrame.Core.UIs
{
    public delegate void OnUIGroupHelperReady(IUIGroupHelper helper);

    public delegate void OnUIGroupHelperReady<T>(T helper) where T : IUIGroupHelper;
}


namespace UnityXFrame.Core.UIs
{
    public delegate void OnUIReady(IUI ui);

    public delegate void OnUIReady<T>(T ui) where T : IUI;
}

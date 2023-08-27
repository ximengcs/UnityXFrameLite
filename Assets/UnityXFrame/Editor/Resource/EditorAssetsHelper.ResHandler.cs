using XFrame.Modules.Resource;

namespace UnityXFrame.Editor
{
    public partial class EditorAssetsHelper
    {
        private class ResHandler : IResHandler
        {
            public object Data { get; }

            public bool IsDone => true;

            public float Pro => 1;

            public ResHandler(object res)
            {
                Data = res;
            }

            public void Start()
            {

            }

            public void Dispose()
            {

            }
        }
    }
}

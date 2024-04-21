using System;
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

            public string AssetPath { get; private set; }

            public Type AssetType { get; private set; }

            public ResHandler(object res, string assetPath, Type type)
            {
                Data = res;
                AssetPath = assetPath;
                AssetType = type;
            }

            public void OnCancel()
            {

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

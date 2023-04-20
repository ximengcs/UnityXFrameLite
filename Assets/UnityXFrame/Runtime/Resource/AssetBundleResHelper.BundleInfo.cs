using UnityEngine;

namespace UnityXFrame.Core.Resource
{
    public partial class AssetBundleResHelper
    {
        private class BundleInfo
        {
            public AssetBundle Bundle;
            public BundleInfo[] Dependencies;

            public string Name { get; }

            public BundleInfo(string name)
            {
                Name = name;
            }

            public Object Load(string resPath, System.Type type)
            {
                InnerCheckState();
                return Bundle.LoadAsset(resPath, type);
            }

            public AssetBundleRequest LoadAsync(string res, System.Type type)
            {
                InnerCheckState();
                return Bundle.LoadAssetAsync(res, type);
            }

            public void Unload()
            {
                Bundle.Unload(true);
            }

            private void InnerCheckState()
            {
                foreach (BundleInfo info in Dependencies)
                    info.InnerCheckState();
                if (Bundle == null)
                    Bundle = AssetBundle.LoadFromFile(Name);
            }
        }
    }
}

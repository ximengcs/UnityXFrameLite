using System;
using UnityEngine;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public partial class AssetBundleResHelper
    {
        private class ResHandler : IResHandler
        {
            private BundleInfo m_Bundle;
            private string m_ResPath;
            private Type m_ResType;

            private AssetBundleRequest m_Request;

            public object Data => m_Request.asset;

            public bool IsDone => m_Request.isDone;

            public float Pro => m_Request.progress;

            public ResHandler(BundleInfo bundle, string resPath, Type resType)
            {
                m_Bundle = bundle;
                m_ResPath = resPath;
                m_ResType = resType;
            }

            public void Start()
            {
                m_Request = m_Bundle.LoadAsync(m_ResPath, m_ResType);
            }

            public void Dispose()
            {
                m_Request = null;
            }
        }
    }
}

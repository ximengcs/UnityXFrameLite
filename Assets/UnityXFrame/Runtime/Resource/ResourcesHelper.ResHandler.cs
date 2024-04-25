using System;
using UnityEngine;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public class ResHandler : IResHandler
    {
        private string m_ResPath;
        private Type m_ResType;
        private ResourceRequest m_Request;

        public object Data => m_Request.asset;

        public bool IsDone
        {
            get
            {
                if (m_Request == null)
                    return false;
                return m_Request.isDone;
            }
        }

        public float Pro => m_Request.progress;

        public string AssetPath => m_ResPath;

        public Type AssetType => m_ResType;

        public ResHandler(string resPath, Type resType)
        {
            m_ResPath = resPath;
            m_ResType = resType;
        }

        public void Start()
        {
            m_Request = Resources.LoadAsync(m_ResPath, m_ResType);
        }

        public void OnCancel()
        {
            Dispose();
        }

        public void Dispose()
        {

        }
    }
}

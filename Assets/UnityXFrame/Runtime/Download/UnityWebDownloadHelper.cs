using UnityEngine.Networking;
using XFrame.Modules.Download;

namespace UnityXFrame.Core.Download
{
    public class DownloadHelper : IDownloadHelper
    {
        private UnityWebRequest m_Request;
        private bool m_Complete;

        public bool IsDone { get; private set; }
        public DownloadResult Result { get; private set; }
        public string Url { get; set; }
        public string[] ReserveUrl { get; set; }

        void IDownloadHelper.OnInit()
        {

        }

        void IDownloadHelper.Request()
        {
            m_Complete = false;
            m_Request = UnityWebRequest.Get(Url);
            m_Request.SendWebRequest();
        }

        void IDownloadHelper.OnUpdate()
        {
            if (m_Complete)
                return;

            if (m_Request == null)
                return;

            if (!m_Request.isDone)
                return;

            IsDone = true;
            Result = new DownloadResult(m_Request.result == UnityWebRequest.Result.Success,
                    m_Request.downloadHandler.text,
                    m_Request.downloadHandler.data,
                    m_Request.error);

            m_Complete = true;
        }

        void IDownloadHelper.OnDispose()
        {
            m_Request?.Dispose();
            m_Request = null;
            IsDone = default;
            Result = default;
        }
    }
}

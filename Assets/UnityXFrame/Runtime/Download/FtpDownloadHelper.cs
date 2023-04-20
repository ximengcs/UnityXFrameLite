using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace XFrame.Modules.Download
{
    public class FtpDownloadHelper : IDownloadHelper
    {
        private FtpWebRequest m_Request;
        private FtpWebResponse m_Response;
        private Task<WebResponse> m_ResponseTask;

        public bool IsDone { get; private set; }
        public DownloadResult Result { get; private set; }
        public string Url { get; set; }

        void IDownloadHelper.OnInit()
        {
        }

        void IDownloadHelper.Request()
        {
            m_Request = (FtpWebRequest)WebRequest.Create(Url);
            m_Request.Method = WebRequestMethods.Ftp.DownloadFile;
            m_ResponseTask = m_Request.GetResponseAsync();
        }

        void IDownloadHelper.OnUpdate()
        {
            if (m_ResponseTask != null)
            {
                if (m_ResponseTask.IsCompleted)
                {
                    if (m_ResponseTask.Status == TaskStatus.RanToCompletion)
                    {
                        m_Response = (FtpWebResponse)m_ResponseTask.Result;
                        Stream responseStream = m_Response.GetResponseStream();
                        BinaryReader reader = new BinaryReader(responseStream);
                        Result = new DownloadResult(m_Response.ContentLength > 0,
                            null, reader?.ReadBytes((int)m_Response.ContentLength),
                            m_Response.StatusDescription);
                        reader.Close();
                        responseStream.Close();
                        m_Response.Close();
                        IsDone = true;
                    }
                    else
                    {
                        IsDone = true;
                        Result = new DownloadResult(false, null, null, m_ResponseTask.Exception.Message);
                    }
                    m_ResponseTask.Dispose();
                    m_ResponseTask = null;
                }
            }
        }

        void IDownloadHelper.OnDispose()
        {
            m_Response?.Dispose();
            m_Request = null;
            IsDone = default;
            Result = default;
        }
    }
}

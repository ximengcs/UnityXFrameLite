using System;
using XFrameShare.Network;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;

namespace Assets.Scripts.Entities
{
    public class TransformMessageHandler : IMessageHandler
    {
        private ClientView m_Viewer;

        public Type Type => typeof(TransformExcuteMessage);

        public void OnInit(IEntity entity)
        {
            m_Viewer = entity.GetCom<ClientView>();
        }

        public void OnDestroy()
        {
            m_Viewer = null;
        }

        public void OnReceive(TransitionData data)
        {
            Log.Debug(NetConst.Net, $"transform excute {data.Message}");
            TransformExcuteMessage message = data.Message as TransformExcuteMessage;
            if (m_Viewer != null)
            {
                m_Viewer.Transform.position = new UnityEngine.Vector3(message.X, message.Y, message.Z);
            }
        }

    }
}

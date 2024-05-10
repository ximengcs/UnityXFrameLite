using System;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class TransformMessageHandler : Entity, IMessageHandler
    {
        private ClientView m_Viewer;

        public Type Type => typeof(TransformExcuteMessage);

        public void Bind(ClientView viewer)
        {
            m_Viewer = viewer;
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

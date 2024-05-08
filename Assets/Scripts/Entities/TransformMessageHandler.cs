using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrameShare.Core.Network;

namespace Assets.Scripts.Entities
{
    public class TransformMessageHandler : Entity, IMessageHandler
    {

        private PlayerView m_Viewer;

        public Type Type => typeof(TransformExcuteMessage);

        public void Bind(PlayerView viewer)
        {
            m_Viewer = viewer;
        }

        public void OnReceive(TransData data)
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

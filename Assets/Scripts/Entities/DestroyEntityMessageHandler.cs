using System;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class DestroyEntityMessageHandler : IMessageConsumer
    {
        private Client m_Client;

        public Type Type => typeof(DestroyEntityMessage);

        public void OnInit(IEntity entity)
        {
            m_Client = entity as Client;
        }

        public void OnDestroy()
        {
            m_Client = null;
        }

        public void OnReceive(TransitionData data)
        {
            DestroyEntityMessage message = data.Message as DestroyEntityMessage;
            Log.Debug(NetConst.Net, $"destroy entity {message.Id}");
            Entry.GetModule<IEntityModule>().Destroy(m_Client);
        }
    }
}

using System;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class DestroyEntityMessageHandler : Entity, IMessageHandler
    {
        private Client m_Client;

        public Type Type => typeof(DestroyEntityMessage);

        protected override void OnInit()
        {
            base.OnInit();
            m_Client = Parent as Client;
        }

        public void OnReceive(TransitionData data)
        {
            DestroyEntityMessage message = data.Message as DestroyEntityMessage;
            Log.Debug(NetConst.Net, $"destroy entity {message.Id} {Id}");
            Entry.GetModule<IEntityModule>().Destroy(m_Client);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class DestroyEntityMessageHandler : Entity, IMessageHandler
    {
        private Player m_Player;

        public Type Type => typeof(DestroyEntityMessage);

        protected override void OnInit()
        {
            base.OnInit();
            m_Player = Parent as Player;
        }

        public void OnReceive(TransData data)
        {
            DestroyEntityMessage message = data.Message as DestroyEntityMessage;
            Log.Debug(NetConst.Net, $"destroy entity {message.Id} {Id}");
            Entry.GetModule<IEntityModule>().Destroy(m_Player);
        }
    }
}

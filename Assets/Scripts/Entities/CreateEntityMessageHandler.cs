using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrame.Modules.Reflection;
using XFrameShare.Core.Network;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class CreateEntityMessageHandler : Entity, IMessageHandler
    {
        public Type Type => typeof(CreateEntity);

        public void OnReceive(TransData data)
        {
            Log.Debug("CreateEntityMessageHandler " + data.FromId + " " + data.ToId);
            ITypeModule typeModule = Entry.GetModule<ITypeModule>();
            CreateEntity message = data.Message as CreateEntity;
            Type type = typeModule.GetType(message.Type);

            IEntity entity = Entry.GetModule<IEntityModule>().Create(type, data.From.Parent);
            entity.AddCom<MailBoxCom>(data.ToId);
        }
    }
}

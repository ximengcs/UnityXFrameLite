using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrame.Modules.Reflection;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class CreateEntityMessageHandler : Entity, IMessageHandler
    {
        public Type Type => typeof(CreateEntityMessage);

        public void OnReceive(TransitionData data)
        {
            ITypeModule typeModule = Entry.GetModule<ITypeModule>();
            CreateEntityMessage message = data.Message as CreateEntityMessage;
            Log.Debug($"CreateEntityMessageHandler {message.Type} {data.FromId} {data.ToId} {message.Id}");
            Type type = typeModule.GetType(message.Type);

            Entry.GetModule<IEntityModule>().Create(type, data.From.Parent, (db) =>
            {
                IEntity entity = db as IEntity;
                entity.AddCom<MailBoxCom>(data.ToId);
            });
        }
    }
}

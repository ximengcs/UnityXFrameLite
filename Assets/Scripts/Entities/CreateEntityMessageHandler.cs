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
            IEntityModule entityModule = Entry.GetModule<IEntityModule>();
            if (entityModule.Get(message.Id) != null)
                return;
            Log.Debug($"CreateEntityMessageHandler {data.FromId} {data.ToId} {message}");
            Type type = typeModule.GetType(message.Type);
            IEntity parent = null;
            if (message.Parent != 0)
                parent = entityModule.Get(message.Parent).Parent;
            entityModule.Create(type, parent, (db) =>
            {
                IEntity entity = db as IEntity;
                entity.AddCom<MailBoxCom>(message.Id);  //在这里添加可以保证初始化中可以拿到
            });
        }
    }
}

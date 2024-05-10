using Google.Protobuf;
using System;
using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class PlayerMoveComponent : Entity, IFactoryMessage
    {
        private Client m_Client;
        private System.Numerics.Vector3 m_PosCache;

        Type IFactoryMessage.Type => typeof(TransformRequestMessage);

        public IMessage Message => new TransformRequestMessage()
        {
            X = m_PosCache.X,
            Y = m_PosCache.Y,
            Z = m_PosCache.Z,
        };

        protected override void OnInit()
        {
            base.OnInit();
            m_Client = Parent as Client;
        }

        public void Up()
        {
            m_PosCache = new System.Numerics.Vector3(0, 1, 0);
            m_Client.Send(this);
        }

        public void Down()
        {
            m_PosCache = new System.Numerics.Vector3(0, -1, 0);
            m_Client.Send(this);
        }

        public void Left()
        {
            m_PosCache = new System.Numerics.Vector3(-1, 0, 0);
            m_Client.Send(this);
        }

        public void Right()
        {
            m_PosCache = new System.Numerics.Vector3(1, 0, 0);
            m_Client.Send(this);
        }
    }
}

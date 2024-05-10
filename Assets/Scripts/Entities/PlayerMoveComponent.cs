using Google.Protobuf;
using System;
using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class PlayerMoveComponent : Entity, IFactoryMessage
    {
        private Client m_Client;

        Type IFactoryMessage.Type => typeof(TransformRequestMessage);

        public IMessage Message => new TransformRequestMessage()
        {
            X = m_Client.Pos.X,
            Y = m_Client.Pos.Y,
            Z = m_Client.Pos.Z,
        };

        protected override void OnInit()
        {
            base.OnInit();
            m_Client = Parent as Client;
        }

        public void Up()
        {
            m_Client.Pos += new System.Numerics.Vector3(0, 1, 0);
            m_Client.Send(this);
        }

        public void Down()
        {
            m_Client.Pos += new System.Numerics.Vector3(0, -1, 0);
            m_Client.Send(this);
        }

        public void Left()
        {
            m_Client.Pos += new System.Numerics.Vector3(-1, 0, 0);
            m_Client.Send(this);
        }

        public void Right()
        {
            m_Client.Pos += new System.Numerics.Vector3(1, 0, 0);
            m_Client.Send(this);
        }
    }
}

using Assets.Scripts.Test;
using Google.Protobuf;
using System;
using XFrame.Modules.Entities;
using XFrameShare.Network;

namespace Assets.Scripts.Entities
{
    public class PlayerMoveComponent : Entity, IFactoryMessage
    {
        private Player m_Player;

        Type IFactoryMessage.Type => typeof(TransformRequestMessage);

        public IMessage Message => new TransformRequestMessage()
        {
            X = m_Player.Pos.X,
            Y = m_Player.Pos.Y,
            Z = m_Player.Pos.Z,
        };

        protected override void OnInit()
        {
            base.OnInit();
            m_Player = Parent as Player;
        }

        public void Up()
        {
            m_Player.Pos += new System.Numerics.Vector3(0, 1, 0);
            m_Player.Send(this);
        }

        public void Down()
        {
            m_Player.Pos += new System.Numerics.Vector3(0, -1, 0);
            m_Player.Send(this);
        }

        public void Left()
        {
            m_Player.Pos += new System.Numerics.Vector3(-1, 0, 0);
            m_Player.Send(this);
        }

        public void Right()
        {
            m_Player.Pos += new System.Numerics.Vector3(1, 0, 0);
            m_Player.Send(this);
        }
    }
}

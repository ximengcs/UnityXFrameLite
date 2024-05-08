using Google.Protobuf;
using System.Numerics;
using UnityEngine;
using XFrame.Modules.Entities;
using XFrameShare.Core.Network;
using XFrameShare.Network;
using XFrameShare.Network.Extension;

namespace Assets.Scripts.Entities
{
    public class PlayerMoveComponent : Entity, IFactoryMessage
    {
        private Player m_Player;

        public IMessage Message => new TransformMessage()
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

        protected override void OnUpdate(float elapseTime)
        {
            base.OnUpdate(elapseTime);
            bool change = false;
            if (Input.GetKeyDown(KeyCode.W))
            {
                m_Player.Pos += new System.Numerics.Vector3(0, 1, 0);
                change = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                m_Player.Pos += new System.Numerics.Vector3(0, -1, 0);
                change = true;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                m_Player.Pos += new System.Numerics.Vector3(1, 0, 0);
                change = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                m_Player.Pos += new System.Numerics.Vector3(-1, 0, 0);
                change = true;
            }

            if (change)
                m_Player.Send(this);
        }
    }
}

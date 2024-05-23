using DG.Tweening;
using Google.Protobuf;
using System;
using UnityEngine;
using XFrame.Core;
using XFrame.Core.Threads;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Entities;
using XFrameShare.Network;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Entities
{
    public class PlayerMoveComponent : IMessageProducer, IMessageConsumer
    {
        private ClientView m_Viewer;
        private Client m_Client;
        private System.Numerics.Vector3 m_PosCache;
        private bool m_Lock;

        Type IMessageProducer.Type => typeof(TransformRequestMessage);

        Type IMessageConsumer.Type => typeof(TransformExcuteMessage);

        public IMessage Message => new TransformRequestMessage()
        {
            X = m_PosCache.X,
            Y = m_PosCache.Y,
            Z = m_PosCache.Z,
        };

        public void OnInit(IEntity entity)
        {
            m_Client = entity as Client;
            m_Viewer = entity.GetCom<ClientView>();
        }

        public void OnDestroy()
        {

        }

        public void Translate(Vector3 target)
        {
            if (m_Lock)
            {
                m_PosCache += new System.Numerics.Vector3(target.x, target.y, target.z);
                return;
            }
            else
            {
                m_PosCache = new System.Numerics.Vector3(target.x, target.y, target.z);
                InnerSyncPos();
            }
        }

        private void InnerSyncPos()
        {
            m_Lock = true;
            m_Client.Send(this);
            m_PosCache = System.Numerics.Vector3.Zero;
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

        public void OnReceive(TransitionData data)
        {
            Log.Debug(NetConst.Net, $"transform excute {data.Message}");
            TransformExcuteMessage message = data.Message as TransformExcuteMessage;
            if (m_Viewer != null)
            {
                Vector3 target = new Vector3(message.X, message.Y, message.Z);
                Entry.GetModule<FiberModule>().MainFiber.Post(InnerMoveHandler, target);
            }
            m_Lock = false;

            if (m_PosCache != System.Numerics.Vector3.Zero)
            {
                InnerSyncPos();
            }
        }

        private void InnerMoveHandler(object state)
        {
            Vector3 target = (Vector3)state;
            m_Viewer.Transform.position = target;
        }
    }
}

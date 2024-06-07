using UnityEngine;
using XFrameShare.Network;
using Assets.Scripts.Test;
using XFrame.Modules.Entities;
using XFrame.Core;
using UnityXFrame.Core.Diagnotics;
using DG.Tweening;
using XFrame.Core.Threads;
using XFrame.Modules.Diagnotics;
using XFrameShare.Network.Tests;

namespace Assets.Scripts.Entities
{
    [NetChildOf(typeof(Client), NetMode.Client)]
    public class ClientView : Entity, IMovementProxy, INetEntityComponent
    {
        private Client m_Client;
        private IMailBox m_ClientMail;
        private GameObject m_Go;
        private SpriteRenderer m_Render;
        private Tween m_MoveTween;

        public Transform Transform => m_Go.transform;

        public void DoMove(Vector3 target, float duration)
        {
            if (m_MoveTween != null)
                m_MoveTween.Kill();
            Transform.DOMove(target, duration).SetEase(Ease.Linear);
        }

        protected override void OnInit()
        {
            base.OnInit();
            m_Client = Parent as Client;
            m_Go = new GameObject(m_Client.Name());
            Transform.localScale = Vector3.one * 0.5f;
            m_Render = m_Go.AddComponent<SpriteRenderer>();
            InnerInit();
        }

        void INetEntityComponent.OnFriendReady()
        {
            
        }

        void INetEntityComponent.OnMasterReady()
        {
            PlayerMoveComponent movement = m_Client.GetHandlerInstance<PlayerMoveComponent>();
            movement.BindProxy(this);
            if (m_Client.IsSelf())
            {
                Global.UI.Get<ControllerUI>().Bind(movement, this);
                Entry.GetModule<Debugger>().AddTitleShower(InnerShowPing);
            }

            m_ClientMail = m_Client.GetCom<IMailBox>(default, false);
        }

        private string InnerShowPing()
        {
            return $"{m_ClientMail.Ping} MS";
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Entry.GetModule<Debugger>().RemoveTitleShower(InnerShowPing);
            Entry.GetModule<FiberModule>().MainFiber.Post(InnerDestroyHandler, null);
        }

        private void InnerDestroyHandler(object state)
        {
            GameObject.Destroy(m_Go);
            m_Go = null;
        }

        private async void InnerInit()
        {
            m_Render.sprite = await Global.Res.LoadAsync<Sprite>("Data2/Textures/QQQ/white.png");
            m_Render.color = new Color[]
            {
                Color.cyan, Color.magenta, Color.red, Color.green, Color.blue, Color.yellow
            }[Random.Range(0, 6)];
            m_Render.sortingOrder = 1;
        }

        public void Move(System.Numerics.Vector3 target)
        {
            Log.Debug($"move to {target}");
            Transform.DOMove(target.ToUnityPos(), 1f);
        }
    }
}

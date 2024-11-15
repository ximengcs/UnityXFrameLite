﻿using Assets.Scripts.Entities;
using System;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityXFrame.Core.UIElements;
using UnityXFrameLib.UIElements;
using XFrame.Core.Threads;
using XFrame.Modules.Entities;
using XFrame.Modules.Times;
using XFrameShare.Network;
using XFrameShare.Network.Tests;

namespace Assets.Scripts.Test
{
    [AutoLoadUI]
    [AutoSpwanUI]
    public class ControllerUI : MonoUI
    {
        public Button m_LeftBtn;
        public Button m_RightBtn;
        public Button m_DownBtn;
        public Button m_UpBtn;
        public Button m_ConnectBtn;
        public Button m_HostBtn;
        public TextMeshProUGUI m_IPText;

        private ClientView m_Client;
        private PlayerMoveComponent m_MoveComponent;

        protected override void OnInit()
        {
            base.OnInit();
            m_ConnectBtn.onClick.AddListener(InnerConnect);
            m_HostBtn.onClick.AddListener(InnerHost);
            m_LeftBtn.onClick.AddListener(InnerLeft);
            m_RightBtn.onClick.AddListener(InnerRight);
            m_DownBtn.onClick.AddListener(InnerDown);
            m_UpBtn.onClick.AddListener(InnerUp);
            m_FixTimer = CDTimer.Create();
            m_FixTimer.Record(0.02f);
        }

        private bool m_Draging;
        private Vector3 m_LastPos;
        private CDTimer m_FixTimer;

        protected override void OnUpdate(double elapseTime)
        {
            base.OnUpdate(elapseTime);
            if (m_Client == null)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (!m_Draging)
                {
                    m_Draging = true;
                    m_LastPos = Input.mousePosition;
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (m_Draging)
                {
                    Vector3 curPos = Input.mousePosition;
                    Vector3 target = Camera.main.ScreenToWorldPoint(curPos) - Camera.main.ScreenToWorldPoint(m_LastPos);

                    if (target != Vector3.zero)
                    {
                        m_MoveComponent.Translate(target.ToStandardPos());
                    }
                    m_LastPos = Input.mousePosition;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_Draging = false;
            }
        }

        public void Bind(PlayerMoveComponent movement, ClientView client)
        {
            m_MoveComponent = movement;
            m_Client = client;
            m_LastPos = m_Client.Transform.position;
        }

        private void InnerConnect()
        {
            IScene gameScene = Global.Scene.Create();
            gameScene.Fiber.Post((state) =>
            {
                var tuple = (ValueTuple<IScene, string>)state;
                Global.Net.Create(tuple.Item1, NetMode.Client, IPAddress.Parse(tuple.Item2), 9999, XProtoType.Tcp);
            }, ValueTuple.Create(gameScene, m_IPText.text));
        }

        private void InnerHost()
        {
            Fiber serverFiber = Global.Fiber.GetOrNew("Server", GameConst.FIBER_ID);
            serverFiber.StartThread(10);
            IScene serverScene = Global.Scene.Create(serverFiber);
            serverScene.Fiber.Post((state) =>
            {
                IScene scene = state as IScene;
                Global.Net.Create(scene, NetMode.Server, 9999, XProtoType.Tcp);
            }, serverScene);
        }

        private void InnerLeft()
        {
            m_MoveComponent?.Left();
        }

        private void InnerRight()
        {
            m_MoveComponent?.Right();
        }

        private void InnerDown()
        {
            m_MoveComponent?.Down();
        }

        private void InnerUp()
        {
            m_MoveComponent?.Up();
        }
    }
}

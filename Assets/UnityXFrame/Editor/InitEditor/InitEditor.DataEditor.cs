﻿using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private interface IDataEditor
        {
            string Name { get; }
            bool Enable { get; set; }
            void OnInit(InitData data);
            void OnUpdate();
            void OnDestroy();
        }

        private abstract class DataEditorBase : IDataEditor
        {
            protected InitData m_Data;

            public bool Enable { get; set; }

            public string Name { get; protected set; }

            public virtual void OnDestroy()
            {

            }

            public void OnInit(InitData data)
            {
                m_Data = data;
                Name = GetType().Name;
                OnInit();
            }

            public virtual void OnUpdate()
            {

            }

            protected virtual void OnInit()
            {

            }
        }
    }
}

using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Event;
using XFrame.Modules.XType;
using System.Collections.Generic;

namespace UnityXFrameLib.Commercial
{
    public class AdsModule : SingletonModule<AdsModule>
    {
        private bool m_Inited;
        private IAdsHelper m_Helper;
        private Action m_InitComplete;
        private XCollection<IAdView> m_Views;
        private Dictionary<int, Type> m_Types;
        private Dictionary<int, Dictionary<int, AdsConfig>> m_Configs;

        public IEventSystem Event { get; private set; }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            Event = EventModule.Inst.NewSys();
            m_Views = new XCollection<IAdView>();
            m_Configs = new Dictionary<int, Dictionary<int, AdsConfig>>();
            m_Types = new Dictionary<int, Type>();
            TypeSystem typeSys = TypeModule.Inst.GetOrNewWithAttr<AdsImplementAttribute>();
            foreach (Type type in typeSys)
            {
                AdsImplementAttribute attr = TypeModule.Inst.GetAttribute<AdsImplementAttribute>(type);
                m_Types.Add(attr.Type, type);
            }
            m_Inited = false;

            typeSys = TypeModule.Inst.GetOrNew<IAdsHelper>();
            foreach (Type type in typeSys)
            {
                m_Helper = (IAdsHelper)TypeModule.Inst.CreateInstance(type);
                m_Helper.OnInit(InnerInitCompleteHandle);
                break;
            }
        }

        public IAdView Open(AdsData data)
        {
            if (m_Types.TryGetValue(data.Type, out Type type))
            {
                IAdView view = m_Views.Get(type, data.EntityId);
                if (view != null)
                {
                    view.Open();
                }
                else
                {
                    if (m_Configs.TryGetValue(data.Type, out Dictionary<int, AdsConfig> configs) && configs.TryGetValue(data.ViewId, out AdsConfig cfg))
                    {
                        view = (IAdView)Activator.CreateInstance(type);
                        view.OnInit(data, cfg);
                        view.Open();
                        m_Views.Add(view);
                    }
                }
                return view;
            }
            return default;
        }

        public void Close(int adType, int entityId = default)
        {
            if (m_Types.TryGetValue(adType, out Type type))
            {
                IAdView view = m_Views.Get(type, entityId);
                if (view != null)
                {
                    view.Close();
                    m_Views.Remove(view);
                }
            }
        }

        public void Register(AdsConfig config)
        {
            if (!m_Configs.TryGetValue(config.Type, out Dictionary<int, AdsConfig> configs))
            {
                configs = new Dictionary<int, AdsConfig>();
                m_Configs.Add(config.Type, configs);
            }
            configs.Add(config.ViewId, config);
        }

        public void WaitInit(Action handler)
        {
            if (m_Inited)
            {
                handler();
            }
            else
            {
                m_InitComplete += handler;
            }
        }

        private void InnerInitCompleteHandle()
        {
            m_Inited = true;
            m_InitComplete?.Invoke();
            m_InitComplete = null;
        }
    }
}

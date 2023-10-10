using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Event;
using System.Collections.Generic;
using XFrame.Modules.Reflection;
using UnityXFrame.Core;

namespace UnityXFrameLib.Commercial
{
    [XType(typeof(IAdsModule))]
    public class AdsModule : ModuleBase, IAdsModule
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
            Event = Global.Event.NewSys();
            m_Views = new XCollection<IAdView>();
            m_Configs = new Dictionary<int, Dictionary<int, AdsConfig>>();
            m_Types = new Dictionary<int, Type>();
            TypeSystem typeSys = Global.Type.GetOrNewWithAttr<AdsImplementAttribute>();
            foreach (Type type in typeSys)
            {
                AdsImplementAttribute attr = Global.Type.GetAttribute<AdsImplementAttribute>(type);
                m_Types.Add(attr.Type, type);
            }
            m_Inited = false;

            typeSys = Global.Type.GetOrNew<IAdsHelper>();
            foreach (Type type in typeSys)
            {
                m_Helper = (IAdsHelper)Global.Type.CreateInstance(type);
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
                    if (GetConfig(data.Type, data.ViewId, out AdsConfig cfg))
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

        public void Close(AdsConfig config)
        {
            Close(config.Type, config.ViewId);
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

        public bool GetConfig(int type, int viewId, out AdsConfig config)
        {
            if (m_Configs.TryGetValue(type, out Dictionary<int, AdsConfig> configs))
            {
                if (configs.TryGetValue(viewId, out config))
                {
                    return true;
                }
                else
                {
                    config = default;
                    return false;
                }
            }
            config = default;
            return false;
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

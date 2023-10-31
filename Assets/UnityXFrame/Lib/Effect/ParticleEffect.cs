using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Pools;
using UnityXFrameLib.Tasks;
using UnityXFrameLib.Utilities;
using XFrame.Modules.Event;

namespace UnityXFrameLib.Effects
{
    public partial class ParticleEffect : IPoolObject
    {
        private int m_TypeId;
        private string m_ResName;
        private Transform m_Root;
        private GameObject m_Inst;
        private Transform m_Tf;
        private ParticleSystem m_Particle;

        private Color m_Color;
        private bool m_Inited;
        private bool m_Playing;
        private Vector3 m_Pos;
        private Vector3 m_Scale;
        private string m_Layer;
        private IEventSystem m_Event;

        private bool m_SetColorDirty;
        private bool m_SetScaleDirty;

        int IPoolObject.PoolKey => m_TypeId;

        public IEventSystem Event => m_Event;
        public string MarkName { get; set; }

        IPool IPoolObject.InPool { get; set; }

        public ParticleEffect(int typeId, string resName, Transform root)
        {
            m_TypeId = typeId;
            m_ResName = resName;
            m_Root = root;
        }

        void IPoolObject.OnCreate()
        {
            m_Event = Global.Event.NewSys();
            m_Inited = false;
            Global.Res.LoadAsync<GameObject>($"{LibConstant.EFFECT_RES_PATH}/{m_ResName}.prefab").OnComplete((prefab) =>
            {
                m_Inst = GameObject.Instantiate(prefab);
                m_Tf = m_Inst.transform;
                m_Tf.SetParent(m_Root);
                m_Particle = m_Inst.GetComponent<ParticleSystem>();
                if (!m_SetColorDirty)
                    m_Color = m_Particle.main.startColor.color;
                if (!m_SetScaleDirty)
                    m_Scale = m_Tf.localScale;
                m_Inited = true;
                InnerRefreshState();
                Event.Trigger(CreateEvent.Create(this));
            }).Start();
        }

        public void SetLayer(string layer)
        {
            m_Layer = layer;
            if (m_Inst != null)
                CommonUtility.SetLayer(m_Inst, LayerMask.NameToLayer(layer));
        }

        public void SetColor(Color color)
        {
            m_SetColorDirty = true;
            m_Color = color;
            InnerRefreshState();
        }

        public void Play(Vector3 target, Vector3 scale)
        {
            m_SetScaleDirty = true;
            m_Pos = target;
            m_Scale = scale;
            m_Playing = true;
            InnerRefreshState();
        }

        public void Play(Vector3 target)
        {
            Play(target, Vector3.one);
        }

        public void Stop()
        {
            if (!m_Playing)
                return;
            m_Playing = false;
            if (m_Particle != null && m_Particle.isPlaying)
            {
                m_Particle.Stop();
            }
        }

        private void InnerRefreshState()
        {
            if (m_Inited)
            {
                if (!string.IsNullOrEmpty(m_Layer))
                {
                    int layerMask = LayerMask.NameToLayer(m_Layer);
                    if (m_Inst.layer != layerMask)
                        CommonUtility.SetLayer(m_Inst, layerMask);
                }

                if (m_Playing)
                {
                    m_Tf.position = m_Pos;
                    m_Tf.localScale = m_Scale;
                    m_Particle.Play();
                    var module = m_Particle.main;
                    if (!module.loop)
                    {
                        TaskExt.Delay(module.duration, InnerDispose);
                    }
                    module.startColor = m_Color;
                }
            }
        }

        private void InnerDispose()
        {
            Event.Trigger(PlayCompleteEvent.Create(this));
            References.Release(this);
        }

        void IPoolObject.OnDelete()
        {

        }

        void IPoolObject.OnRelease()
        {
            m_Particle.Stop();
            m_Inst?.SetActive(false);
            Event.Trigger(DestroyEvent.Create(this));
        }

        void IPoolObject.OnRequest()
        {
            m_Playing = false;
            m_SetColorDirty = false;
            m_SetScaleDirty = false;
            Event.Unlisten();
            if (m_Inst != null)
            {
                m_Inst.SetActive(true);
                Event.Trigger(CreateEvent.Create(this));
            }
        }
    }
}

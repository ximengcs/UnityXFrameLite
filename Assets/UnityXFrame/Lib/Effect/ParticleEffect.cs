using UnityEngine;
using XFrame.Modules.Pools;
using UnityXFrameLib.Tasks;
using XFrame.Modules.Resource;
using UnityXFrameLib.Utilities;

namespace UnityXFrameLib.Effects
{
    public class ParticleEffect : IPoolObject
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

        int IPoolObject.PoolKey => m_TypeId;

        public ParticleEffect(int typeId, string resName, Transform root)
        {
            m_TypeId = typeId;
            m_ResName = resName;
            m_Root = root;
        }

        void IPoolObject.OnCreate()
        {
            m_Inited = false;
            ResModule.Inst.LoadAsync<GameObject>($"{LibConstant.EFFECT_RES_PATH}/{m_ResName}.prefab").OnComplete((prefab) =>
            {
                m_Inst = GameObject.Instantiate(prefab);
                m_Tf = m_Inst.transform;
                m_Tf.SetParent(m_Root);
                m_Particle = m_Inst.GetComponent<ParticleSystem>();
                CommonUtility.SetLayer(m_Inst, LayerMask.NameToLayer(LibConstant.EFFECT_LAYER));
                m_Inited = true;
                InnerRefreshState();
            }).Start();
        }

        public void SetColor(Color color)
        {
            m_Color = color;
            InnerRefreshState();
        }

        public void Play(Vector3 target, Vector3 scale)
        {
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
            References.Release(this);
        }

        void IPoolObject.OnDelete()
        {

        }

        void IPoolObject.OnRelease()
        {
            m_Particle.Stop();
            m_Inst?.SetActive(false);
        }

        void IPoolObject.OnRequest()
        {
            m_Playing = false;
            m_Inst?.SetActive(true);
        }
    }
}

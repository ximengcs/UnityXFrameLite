using UnityEngine;
using XFrame.Modules.Pools;
using UnityXFrameLib.Tasks;
using XFrame.Modules.Resource;
using UnityXFrameLib.Utilities;
using UnityEngine.UIElements;
using XFrame.Core;

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

        private bool m_SetColorDirty;
        private bool m_SetScaleDirty;

        int IPoolObject.PoolKey => m_TypeId;

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
            m_Inited = false;
            XModule.Res.LoadAsync<GameObject>($"{LibConstant.EFFECT_RES_PATH}/{m_ResName}.prefab").OnComplete((prefab) =>
            {
                m_Inst = GameObject.Instantiate(prefab);
                m_Tf = m_Inst.transform;
                m_Tf.SetParent(m_Root);
                m_Particle = m_Inst.GetComponent<ParticleSystem>();
                if (!m_SetColorDirty)
                    m_Color = m_Particle.main.startColor.color;
                if (!m_SetScaleDirty)
                    m_Scale = m_Tf.localScale;
                CommonUtility.SetLayer(m_Inst, LayerMask.NameToLayer(LibConstant.EFFECT_LAYER));
                m_Inited = true;
                InnerRefreshState();
            }).Start();
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
            m_SetColorDirty = false;
            m_SetScaleDirty = false;
            m_Inst?.SetActive(true);
        }
    }
}

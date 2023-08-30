using System;
using UnityEngine;
using XFrame.Modules.Pools;
using System.Collections.Generic;

namespace UnityXFrameLib.Effects
{
    [PoolHelper(typeof(ParticleEffect))]
    public class ParticleEffectPoolHelper : IPoolHelper
    {
        private Transform m_PoolRoot;
        private Dictionary<string, Transform> m_Roots;

        int IPoolHelper.CacheCount => 64;

        public ParticleEffectPoolHelper()
        {
            GameObject inst = new GameObject($"[Pool]ParticleEffect");
            m_PoolRoot = inst.transform;
            m_Roots = new Dictionary<string, Transform>();
        }

        private Transform InnerGetEffectRoot(string key)
        {
            if (!m_Roots.TryGetValue(key, out Transform root))
            {
                GameObject inst = new GameObject($"[Root]{key}");
                root = inst.transform;
                root.SetParent(m_PoolRoot, false);
                m_Roots[key] = root;
            }
            return root;
        }

        IPoolObject IPoolHelper.Factory(Type type, int poolKey, object userData)
        {
            string resName = (string)userData;
            return new ParticleEffect(poolKey, resName, InnerGetEffectRoot(resName));
        }

        void IPoolHelper.OnObjectCreate(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectDestroy(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectRelease(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectRequest(IPoolObject obj)
        {

        }
    }
}

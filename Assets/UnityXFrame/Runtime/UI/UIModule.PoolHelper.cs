using System;
using UnityEngine;
using XFrame.Modules.Pools;
using XFrame.Modules.Resource;
using UnityXFrame.Core.Resource;

namespace UnityXFrame.Core.UIs
{
    public partial class UIModule
    {
        private class UIPoolHelper : IPoolHelper
        {
            int IPoolHelper.CacheCount => 8;

            IPoolObject IPoolHelper.Factory(Type type, int poolKey, object userData)
            {
                bool useNative = (bool)userData;
                GameObject prefab;
                string uiPath = $"{Constant.UI_RES_PATH}/{type.Name}.prefab";
                if (useNative)
                    prefab = NativeResModule.Inst.Load<GameObject>(uiPath);
                else
                    prefab = ResModule.Inst.Load<GameObject>(uiPath);
                GameObject inst = GameObject.Instantiate(prefab);

                IUI ui;
                if (type.BaseType == typeof(UI))
                    ui = InnerInstantiateUI(inst, type);
                else
                    ui = InnerInstantiateMonoUI(inst, type);
                inst.name = $"{ui.GetType().Name}{ui.GetHashCode()}";
                return ui;

            }

            private UI InnerInstantiateUI(GameObject inst, Type type)
            {
                UI ui = (UI)Activator.CreateInstance(type);
                ui.m_Root = inst;
                return ui;
            }

            private MonoUI InnerInstantiateMonoUI(GameObject inst, Type type)
            {
                MonoUI ui = (MonoUI)inst.GetComponent(type);
                if (ui == null)
                    ui = (MonoUI)inst.AddComponent(type);
                return ui;
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
}

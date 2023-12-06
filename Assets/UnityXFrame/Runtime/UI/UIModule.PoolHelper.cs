using System;
using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Pools;
using XFrame.Modules.Resource;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIElements
{
    public partial class UIModule
    {
        private interface IUIPoolHelper : IPoolHelper
        {
            ITask PreloadRes(IEnumerable<Type> types, int useResModule);
        }

        private class DefaultUIPoolHelper : IUIPoolHelper
        {
            int IPoolHelper.CacheCount => 8;

            ITask IUIPoolHelper.PreloadRes(IEnumerable<Type> types, int useResModule)
            {
                List<string> uiPaths = new List<string>();
                foreach (Type type in types)
                    uiPaths.Add(InnerUIPath(type));
                return Entry.GetModule<IResModule>(useResModule).Preload<GameObject>(uiPaths);
            }

            IPoolObject IPoolHelper.Factory(Type type, int poolKey, object userData)
            {
                int useResModule = (int)userData;
                string uiPath = InnerUIPath(type);
                GameObject prefab = Entry.GetModule<IResModule>(useResModule).Load<GameObject>(uiPath);
                if (prefab == null)
                    throw new Exception($"UI prefab is null, {uiPath} {type.FullName}");
                GameObject inst = GameObject.Instantiate(prefab);

                IUI ui;
                if (type.BaseType == typeof(UI))
                    ui = InnerInstantiateUI(inst, type);
                else
                    ui = InnerInstantiateMonoUI(inst, type);
                inst.name = $"{ui.GetType().Name}{ui.GetHashCode()}";
                return ui;

            }

            private string InnerUIPath(Type type)
            {
                return $"{Constant.UI_RES_PATH}/{type.Name}.prefab";
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

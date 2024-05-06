
using System;
using System.Collections.Generic;
using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Entities;
using XFrame.Modules.Reflection;

namespace Assets.Scripts.Entities
{
    public static class NetEntityUtility
    {
        private static Dictionary<Type, Type> m_ViewMap = new Dictionary<Type, Type>();

        public static void AddView(this IEntity entity)
        {
            Type entityType = entity.GetType();
            ITypeModule typeModule = Entry.GetModule<ITypeModule>();
            if (!m_ViewMap.TryGetValue(entityType, out Type type))
            {
                TypeSystem typeSys = typeModule.GetOrNewWithAttr<EntityViewAttribute>();
                foreach (Type target in typeSys)
                {
                    EntityViewAttribute attr = typeModule.GetAttribute<EntityViewAttribute>(target);
                    if (attr != null)
                    {
                        m_ViewMap.Add(target, attr.Target);
                        if (entityType == attr.Target)
                            type = target;
                    }
                }
            }
            if (type != null)
            {
                if (typeof(MonoBehaviour).IsAssignableFrom(type))
                {
                    GameObject go = new GameObject(type.Name);
                    IEntityViewer viewer = go.AddComponent(type) as IEntityViewer;
                    if (viewer != null)
                        viewer.OnInit(entity);
                }
                else
                {
                    IEntityViewer viewer = typeModule.CreateInstance(type) as IEntityViewer;
                    if (viewer != null)
                        viewer.OnInit(entity);
                }
            }
        }
    }
}

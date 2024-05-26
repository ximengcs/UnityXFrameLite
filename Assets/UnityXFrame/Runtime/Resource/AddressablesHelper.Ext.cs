using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper
    {
        private delegate AsyncOperationHandle<T> LoadAssetFunc<T>(object path);

        private static class Ext
        {
            private static Type m_LoadAssetFuncType;
            private static MethodInfo s_LoadAssetAsync;
            private static Dictionary<Type, Delegate> s_LoadAssetDelegates;

            public static void OnInit()
            {
                m_LoadAssetFuncType = typeof(LoadAssetFunc<>);
                s_LoadAssetDelegates = new Dictionary<Type, Delegate>();
                Type type = typeof(Addressables);
                foreach (MethodInfo info in type.GetMethods())
                {
                    if (info.Name == "LoadAssetAsync" && info.GetParameters()[0].ParameterType == typeof(object))
                    {
                        s_LoadAssetAsync = info;
                        break;
                    }
                }
            }

            public static object LoadAssetAsync(string resPath, Type resType)
            {
                if (!s_LoadAssetDelegates.TryGetValue(resType, out Delegate fun))
                {
                    Type realFun = m_LoadAssetFuncType.MakeGenericType(resType);
                    MethodInfo realMethod = s_LoadAssetAsync.MakeGenericMethod(resType);
                    fun = realMethod.CreateDelegate(realFun);
                    s_LoadAssetDelegates.Add(resType, fun);
                }

                return fun.DynamicInvoke(new object[] { resPath });
            }
        }
    }
}

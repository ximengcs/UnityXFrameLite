
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper
    {
        private class ReflectResHandler : IAddresableResHandler
        {
            private object m_Handle;
            private object m_Data;

            private static PropertyInfo s_Result;
            private static PropertyInfo s_IsDone;
            private static PropertyInfo s_PercentComplete;
            private static MethodInfo s_WaitComplete;

            public object Data
            {
                get
                {
                    if (m_Data == null)
                        m_Data = InnerResult(m_Handle);
                    return m_Data;
                }
            }

            public bool IsDone => InnerIsDone(m_Handle);

            public float Pro => InnerPercentComplete(m_Handle);

            public ReflectResHandler(object handle, Type resType)
            {
                m_Handle = handle;

                Type handleType = typeof(AsyncOperationHandle<>);
                Type realType = handleType.MakeGenericType(resType);
                s_Result = realType.GetProperty("Result");
                s_IsDone = realType.GetProperty("IsDone");
                s_PercentComplete = realType.GetProperty("PercentComplete");
                s_WaitComplete = realType.GetMethod("WaitForCompletion", BindingFlags.Public & BindingFlags.Instance);
            }

            public void Start()
            {
                InnerStart();
            }

            public void Dispose()
            {

            }

            public void Release()
            {
                Addressables.Release(m_Handle);
                m_Data = null;
            }

            private void InnerStart()
            {
                m_Data = InnerWaitComplete(m_Handle);
            }

            private object InnerResult(object handle)
            {
                return s_Result.GetValue(handle);
            }

            private static bool InnerIsDone(object handle)
            {
                return (bool)s_IsDone.GetValue(handle);
            }

            private static float InnerPercentComplete(object handle)
            {
                return (float)s_PercentComplete.GetValue(handle);
            }

            private static object InnerWaitComplete(object handle)
            {
                return s_WaitComplete.Invoke(handle, null);
            }
        }
    }
}

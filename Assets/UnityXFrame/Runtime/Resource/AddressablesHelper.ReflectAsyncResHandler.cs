using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper
    {
        private class ReflectAsyncResHandler : IAddresableResHandler
        {
            private object m_Handle;
            private object m_Data;

            private static PropertyInfo s_Result;
            private static PropertyInfo s_IsDone;
            private static PropertyInfo s_PercentComplete;
            private static PropertyInfo s_Task;

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

            public ReflectAsyncResHandler(object handle, Type resType)
            {
                m_Handle = handle;

                Type handleType = typeof(AsyncOperationHandle<>);
                Type realType = handleType.MakeGenericType(resType);
                s_Result = realType.GetProperty("Result");
                s_IsDone = realType.GetProperty("IsDone");
                s_PercentComplete = realType.GetProperty("PercentComplete");
                s_Task = realType.GetProperty("Task");
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

            private async void InnerStart()
            {
                await InnerTask(m_Handle);
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

            private static Task InnerTask(object handle)
            {
                return (Task)s_Task.GetValue(handle);
            }
        }
    }
}

using System;
using UnityEngine;
using XFrame.Modules.Tasks;
using XFrame.Modules.XType;
using XFrame.Modules.Pools;
using XFrame.Core;

namespace UnityXFrameLib.Pools
{
    public static class PoolExt
    {
        public static ITask CollectSpwanTask()
        {
            ActionTask task = XModule.Task.GetOrNew<ActionTask>();
            TypeSystem typeSys = XModule.Type.GetOrNewWithAttr<AutoSpwanAttribute>();
            foreach (Type type in typeSys)
            {
                task.Add(() =>
                {
                    Debug.LogWarning(type.GetHashCode());
                    AutoSpwanAttribute attr = XModule.Type.GetAttribute<AutoSpwanAttribute>(type);
                    IPool pool = XModule.Pool.GetOrNew(type);
                    pool.Spawn(attr.PoolKey, attr.Count, attr.UserData);
                });
            }
            return task;
        }
    }
}

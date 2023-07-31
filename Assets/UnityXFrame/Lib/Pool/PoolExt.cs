using System;
using UnityEngine;
using XFrame.Modules.Tasks;
using XFrame.Modules.XType;
using XFrame.Modules.Pools;

namespace UnityXFrameLib.Pools
{
    public static class PoolExt
    {
        public static ITask CollectSpwanTask()
        {
            ActionTask task = TaskModule.Inst.GetOrNew<ActionTask>();
            TypeSystem typeSys = TypeModule.Inst.GetOrNewWithAttr<AutoSpwanAttribute>();
            foreach (Type type in typeSys)
            {
                task.Add(() =>
                {
                    Debug.LogWarning(type.GetHashCode());
                    AutoSpwanAttribute attr = TypeModule.Inst.GetAttribute<AutoSpwanAttribute>(type);
                    IPool pool = PoolModule.Inst.GetOrNew(type);
                    pool.Spawn(attr.PoolKey, attr.Count, attr.UserData);
                });
            }
            return task;
        }
    }
}

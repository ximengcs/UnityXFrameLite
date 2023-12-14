using System;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Pools;
using XFrame.Modules.Reflection;

namespace UnityXFrameLib.Pools
{
    public static class PoolExt
    {
        public static ITask CollectSpwanTask()
        {
            ActionTask task = Global.Task.GetOrNew<ActionTask>();
            TypeSystem typeSys = Global.Type.GetOrNewWithAttr<AutoSpwanAttribute>();
            foreach (Type type in typeSys)
            {
                task.Add(() =>
                {
                    AutoSpwanAttribute attr = Global.Type.GetAttribute<AutoSpwanAttribute>(type);
                    IPool pool = Global.Pool.GetOrNew(type);
                    pool.Spawn(attr.PoolKey, attr.Count, attr.UserData);
                });
            }
            return task;
        }
    }
}

using System;
using XFrame.Modules.Pools;
using XFrame.Modules.Reflection;
using XFrame.Tasks;

namespace UnityXFrameLib.Pools
{
    public static class PoolExt
    {
        public static async XTask CollectSpwanTask()
        {
            TypeSystem typeSys = Global.Type.GetOrNewWithAttr<AutoSpwanAttribute>();
            foreach (Type type in typeSys)
            {
                AutoSpwanAttribute attr = Global.Type.GetAttribute<AutoSpwanAttribute>(type);
                IPool pool = Global.Pool.GetOrNew(type);
                pool.Spawn(attr.PoolKey, attr.Count, attr.UserData);
                await XTask.NextFrame();
            }
        }
    }
}

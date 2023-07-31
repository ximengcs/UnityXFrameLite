
using System;

namespace UnityXFrameLib.Pools
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoSpwanAttribute : Attribute
    {
        public int PoolKey { get; }
        public int Count { get; }
        public object UserData { get; }

        public AutoSpwanAttribute(int poolKey, int count, object userData = null)
        {
            PoolKey = poolKey;
            Count = count;
            UserData = userData;
        }

        public AutoSpwanAttribute(int count = 1, object userData = null)
        {
            PoolKey = default;
            Count = count;
            UserData = userData;
        }

        public AutoSpwanAttribute(object userdata)
        {
            PoolKey = default;
            Count = 1;
            UserData = userdata;
        }
    }
}

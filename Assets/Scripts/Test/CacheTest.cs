
using XFrame.Modules.Caches;
using XFrame.Tasks;

namespace XFrame.Core.Caches
{
    public class CacheTest : ICacheObject
    {
        public override string ToString()
        {
            return $"CacheTest_{GetHashCode()}";
        }
    }

    [CacheObject(typeof(CacheTest))]
    public class CacheTestFactory : ICacheObjectFactory
    {
        public bool IsDone { get; private set; }

        public ICacheObject Result { get; private set; }

        public object Data => null;

        public float Pro => IsDone ? XTaskHelper.MAX_PROGRESS : XTaskHelper.MIN_PROGRESS;

        public void OnCancel()
        {
            OnFinish();
        }

        public void OnFactory()
        {
            IsDone = true;
            Result = new CacheTest();
        }

        public void OnFinish()
        {
            IsDone = false;
            Result = null;
        }
    }
}

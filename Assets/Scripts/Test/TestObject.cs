
using XFrame.Modules.Pools;

namespace Assets.Scripts.Test
{
    public class TestObject2 : TestObject
    {

    }

    public class TestObject : IPoolObject
    {
        int IPoolObject.PoolKey => throw new System.NotImplementedException();

        string IPoolObject.MarkName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        IPool IPoolObject.InPool { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Test()
        {

        }

        void IPoolObject.OnCreate(IPoolModule module)
        {
            throw new System.NotImplementedException();
        }

        void IPoolObject.OnDelete()
        {
            throw new System.NotImplementedException();
        }

        void IPoolObject.OnRelease()
        {
            throw new System.NotImplementedException();
        }

        void IPoolObject.OnRequest()
        {
            throw new System.NotImplementedException();
        }
    }
}

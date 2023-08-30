using XFrame.Modules.Containers;
using XFrame.Modules.Pools;

namespace UnityXFrame.Core.UIElements
{
    public partial class MonoUI
    {
        private class MonoContainer : Container
        {
            public void TriggerOnCreateFromPool()
            {
                InnerOnCreate();
                OnCreateFromPool();
            }

            public void TriggerOnDestroyFromPool()
            {
                OnDestroyFromPool();
                InnerOnDelete();
            }

            public void TriggerOnRequestFromPool()
            {
                InnerOnRequest();
                OnRequestFromPool();
            }

            public void TriggerOnReleaseFromPool()
            {
                OnReleaseFromPool();
                InnerOnRelease();
            }
        }
    }
}

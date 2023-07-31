using UnityEngine;

namespace UnityXFrame.Core
{
    [DefaultExecutionOrder(Constant.EXECORDER_AFTER)]
    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        public static T Inst { get; private set; }

        public SingletonMono()
        {
            Inst = (T)this;
        }
    }
}

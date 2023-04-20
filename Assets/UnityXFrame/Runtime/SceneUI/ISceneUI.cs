using UnityEngine;
using XFrame.Modules.Entities;

namespace UnityXFrame.Core.SceneUIs
{
    /// <summary>
    /// SceneUI接口
    /// </summary>
    public interface ISceneUI
    {
        /// <summary>
        /// 打开SceneUI
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭SceneUI
        /// </summary>
        void Close();

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="entity">SceneUI所属Entity实体</param>
        /// <param name="root">SceneUI根GameObject节点的变换组件</param>
        void OnInit(Entity entity, RectTransform root);

        /// <summary>
        /// 更新生命周期
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        void OnDestroy();
    }
}

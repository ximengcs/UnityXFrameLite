using System;
using System.IO;
using UnityEngine;
using XFrame.Core;
using UnityEngine.UI;
using XFrame.Utility;
using System.Collections.Generic;
using XFrame.Modules.Entities;
using XFrame.Modules.Resource;
using UnityXFrame.Core.Entities;
using XFrame.Modules.XType;

namespace UnityXFrame.Core.SceneUIs
{
    /// <summary>
    /// SceneUI模块
    /// </summary>
    [XModule]
    public partial class SceneUIModule : SingletonModule<SceneUIModule>, IUpdater
    {
        #region Inner Field
        private List<ISceneUI> m_Actives;
        private Dictionary<int, SceneUIInfo> m_ActivesMap;
        private List<ISceneUI> m_OnlyOneOpens;
        private ISceneUI m_CurOneOpen;
        #endregion

        #region Interface
        /// <summary>
        /// 为Entity实体创建一个创建好的SceneUI
        /// </summary>
        /// <typeparam name="T">场景UI的拥有者</typeparam>
        /// <param name="entity">Entity实体</param>
        /// <returns>创建好的SceneUI</returns>
        public T Create<T>(Entity entity) where T : ISceneUI
        {
            Type uiType = typeof(T);
            RectTransform tf = InnerGenCanvas(uiType, entity);
            string resPath = Path.Combine(Constant.SCENEUI_RES_PATH, uiType.Name);
            GameObject prefab = ResModule.Inst.Load<GameObject>(resPath);
            GameObject inst = GameObject.Instantiate(prefab, tf);
            T ui = (T)Activator.CreateInstance(uiType);
            InnerInitLayer(ui, inst);
            ui.OnInit(entity, inst.GetComponent<RectTransform>());

            m_Actives.Add(ui);
            SceneUIInfo info = new SceneUIInfo(ui, inst);
            m_ActivesMap.Add(info.Id, info);

            return ui;
        }

        /// <summary>
        /// 更新SceneUI打开状态，SceneUI打开后会自动调用一次
        /// </summary>
        /// <param name="ui">需要更新状态的SceneUI</param>
        public void UpdateUIState(ISceneUI ui)
        {
            if (ui == m_CurOneOpen)
                return;

            if (ui == null)
            {
                m_CurOneOpen.Close();
                m_CurOneOpen = null;
                return;
            }

            if (TypeModule.Inst.HasAttribute<OnlyOneOpenAttribute>(ui.GetType()))
            {
                if (m_CurOneOpen != null)
                    m_OnlyOneOpens.Add(m_CurOneOpen);
                foreach (ISceneUI willUI in m_OnlyOneOpens)
                    willUI.Close();
                m_OnlyOneOpens.Clear();

                m_CurOneOpen = ui;
            }
        }

        /// <summary>
        /// 销毁SceneUI，销毁时调用此方法，经过一些处理后自动调用SceneUI的OnDestroy生命周期方法
        /// </summary>
        /// <param name="ui">需要销毁的UI</param>
        public void Destroy(ISceneUI ui)
        {
            int id = ui.GetHashCode();
            if (m_ActivesMap.TryGetValue(id, out SceneUIInfo info))
            {
                ui.OnDestroy();
                GameObject.Destroy(info.UnityInst);
                m_ActivesMap.Remove(id);
                m_Actives.Remove(ui);
                m_OnlyOneOpens.Remove(ui);
                if (m_CurOneOpen == ui)
                    m_CurOneOpen = null;
            }
        }
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Actives = new List<ISceneUI>();
            m_ActivesMap = new Dictionary<int, SceneUIInfo>();
            m_OnlyOneOpens = new List<ISceneUI>();
        }

        public void OnUpdate(float escapeTime)
        {
            if (m_Actives.Count <= 0)
                return;

            foreach (ISceneUI inst in m_Actives)
                inst.OnUpdate();
        }

        protected override void OnDestroy()
        {
            if (m_Actives.Count <= 0)
                return;

            foreach (ISceneUI inst in m_Actives)
                inst.OnDestroy();
            m_Actives = null;
            m_ActivesMap = null;
        }
        #endregion

        #region Inner Implement
        private RectTransform InnerGenCanvas(Type uiType, Entity entity)
        {
            Transform tf;
            GameObjectCom com = entity.GetCom<GameObjectCom>();
            if (TypeModule.Inst.HasAttribute<NoneInteractiveAttribute>(uiType))
            {
                tf = com.Tf.Find("NotInterActUI");
                if (tf == null)
                    tf = new GameObject("NotInterActUI", typeof(Canvas)).transform;
                else
                    return tf as RectTransform;
            }
            else
            {
                tf = com.Tf.Find("UI");
                if (tf == null)
                    tf = new GameObject("UI", typeof(Canvas), typeof(GraphicRaycaster)).transform;
                else
                    return tf as RectTransform;
            }

            Canvas canvas = tf.GetComponent<Canvas>();
            tf.SetParent(com.Tf);
            tf.localScale /= Constant.UNIT_PIXEL;
            RectTransform rectTf = tf as RectTransform;
            canvas.sortingLayerID = Constant.SCENEUI_SORT_LAYER;
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            rectTf.anchoredPosition3D = Vector3.zero;
            rectTf.sizeDelta = new Vector2(Constant.SIZE_X, Constant.SIZE_Y);
            return rectTf;
        }

        private void InnerInitLayer(ISceneUI ui, GameObject unityInst)
        {
            RectTransform tf = unityInst.GetComponent<RectTransform>();
            SceneUILayer layer = TypeModule.Inst.GetAttribute<SceneUILayer>(ui.GetType());
            Vector3 pos = tf.anchoredPosition3D;
            pos.z = layer != null ? -layer.Layer : 0;
            tf.anchoredPosition3D = pos;
        }
        #endregion
    }
}

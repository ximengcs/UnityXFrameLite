using UnityEngine;
using UnityXFrame.Core.UIElements;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using XFrame.Core;
using UnityXFrame.Core;

namespace UnityXFrameLib.UIElements
{
    public class DragUICom : UICom
    {
        private PointerEventData m_EvtData;
        private List<RaycastResult> m_Results;
        private bool m_Draging;
        private RectTransform m_MoveTarget;
        private RectTransform m_DragTarget;
        private Vector2 m_OrgPos;
        private Vector2 m_CurPos;

        public void Reset()
        {
            m_MoveTarget.anchoredPosition = m_OrgPos;
        }

        public static void SetTarget(IDataProvider dataProvider, string moveTarget, string dragTarget)
        {
            SetMoveTarget(dataProvider, moveTarget);
            SetDragTarget(dataProvider, dragTarget);
        }

        public static void SetMoveTarget(IDataProvider dataProvider, string targetName)
        {
            dataProvider.SetData("MoveTarget", targetName);
        }

        public static void SetDragTarget(IDataProvider dataProvider, string targetName)
        {
            dataProvider.SetData("DragTarget", targetName);
        }

        protected override void OnInit()
        {
            base.OnInit();
            m_Draging = false;
            m_EvtData = new PointerEventData(EventSystem.current);
            m_Results = new List<RaycastResult>();

            string move = GetData<string>("MoveTarget");
            string drag = GetData<string>("DragTarget");
            if (!string.IsNullOrEmpty(move))
                m_MoveTarget = m_UIFinder.GetUI<RectTransform>(move);
            if (!string.IsNullOrEmpty(drag))
                m_DragTarget = m_UIFinder.GetUI<RectTransform>(drag);
            if (m_MoveTarget == null)
            {
                IUI ui = Master as IUI;
                m_MoveTarget = ui.Root;
            }
            m_OrgPos = m_MoveTarget.anchoredPosition;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            m_Results.Clear();
            m_MoveTarget = null;
            m_DragTarget = null;
        }

        protected override void OnUpdate(double elapseTime)
        {
            base.OnUpdate(elapseTime);
            if (m_MoveTarget == null || m_DragTarget == null)
                return;

            if (EventSystem.current == null)
                return;

            Vector2 lastPos = m_CurPos;
            m_CurPos = Input.mousePosition;

            if (m_Draging)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 pos = (m_CurPos - lastPos) * Global.UI.PixelScale;
                    m_MoveTarget.anchoredPosition += pos;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    m_Draging = false;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_Results.Clear();
                    m_EvtData.position = Input.mousePosition;
                    EventSystem.current.RaycastAll(m_EvtData, m_Results);
                    if (m_Results.Count > 0)
                    {
                        RaycastResult r = m_Results[0];
                        if (r.gameObject.transform == m_DragTarget)
                            m_Draging = true;
                    }
                }

            }
        }
    }
}

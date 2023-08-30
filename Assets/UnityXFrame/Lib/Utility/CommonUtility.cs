using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UnityXFrameLib.Utilities
{
    public class CommonUtility
    {
        public static Color ParseColor(string colorStr)
        {
            ColorUtility.TryParseHtmlString(colorStr, out Color color);
            return color;
        }
        
        public static void SetLayer(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform t in gameObject.transform)
                SetLayer(t.gameObject, layer);
        }

        private static PointerEventData m_PointerData;
        private static List<RaycastResult> m_RayResults;
        public static bool IsPointerOverUIObject(Vector2 screenPosition)
        {
            if (EventSystem.current == null)
                return true;
            if (m_PointerData == null)
            {
                m_PointerData = new PointerEventData(EventSystem.current);
                m_RayResults = new List<RaycastResult>();
            }

            m_RayResults.Clear();
            m_PointerData.position = new Vector2(screenPosition.x, screenPosition.y);
            EventSystem.current.RaycastAll(m_PointerData, m_RayResults);
            return m_RayResults.Count > 0;
        }
    }
}

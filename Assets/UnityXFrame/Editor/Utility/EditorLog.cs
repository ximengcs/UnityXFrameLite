using UnityEngine;

namespace UnityXFrame.Editor
{
    public class EditorLog
    {
        public static string Color(object msg, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>";
        }

        public static void Debug(object msg)
        {
            UnityEngine.Debug.Log($"<color=yellow>[UnityXFrame]</color><color=cyan>{msg}</color>");
        }

        public static void Error(object msg)
        {
            UnityEngine.Debug.LogError(msg);
        }
    }
}

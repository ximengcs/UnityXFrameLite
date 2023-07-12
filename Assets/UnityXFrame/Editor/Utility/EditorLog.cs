
namespace UnityXFrame.Editor
{
    public class EditorLog
    {
        public static void Debug(object msg)
        {
            UnityEngine.Debug.Log($"<color=yellow>[UnityXFrame]</color><color=orange>{msg}</color>");
        }
    }
}

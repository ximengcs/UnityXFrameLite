namespace UnityXFrame.Core.UIElements
{
    public static class UIHelper
    {
        private static int s_MODULE_ID = 1000;
        internal static int ManagerId => s_MODULE_ID++;

        public static string GetString(this UICom uiCom)
        {
            return uiCom.GetData<string>(uiCom.GetType().Name);
        }
    }
}
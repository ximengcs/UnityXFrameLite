using TMPro;
using System;
using XFrame.Modules.Event;
using XFrame.Modules.Local;
using UnityXFrame.Core.Resource;
using System.Collections.Generic;

namespace UnityXFrameLib.Localize
{
    public static class LocalizeExt
    {
        private static Dictionary<int, XEventHandler> m_LangChangeHandlers = new Dictionary<int, XEventHandler>();

        public static void RegisterLocalText(TextMeshProUGUI textCom, Action<TextMeshProUGUI> handler)
        {
            int code = textCom.GetHashCode();
            if (m_LangChangeHandlers.ContainsKey(code))
                UnregisterLocalText(textCom);

            XEventHandler langChangeHandler = (e) =>
            {
                textCom.font = LoadFont(LocalizeModule.Inst.Lang);
                handler?.Invoke(textCom);
            };
            LocalizeModule.Inst.Event.Listen(LanguageChangeEvent.EventId, langChangeHandler);
            m_LangChangeHandlers.Add(code, langChangeHandler);
            langChangeHandler?.Invoke(null);
        }

        public static void UnregisterLocalText(TextMeshProUGUI textCom)
        {
            int code = textCom.GetHashCode();
            if (m_LangChangeHandlers.TryGetValue(code, out XEventHandler handler))
            {
                LocalizeModule.Inst.Event.Unlisten(LanguageChangeEvent.EventId, handler);
                m_LangChangeHandlers.Remove(code);
            }
        }

        public static TMP_FontAsset LoadFont(Language lang)
        {
            return NativeResModule.Inst.Load<TMP_FontAsset>($"Fonts & Materials/{lang}");
        }
    }
}

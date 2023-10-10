using TMPro;
using System;
using XFrame.Modules.Event;
using XFrame.Modules.Local;
using UnityXFrame.Core.Resource;
using System.Collections.Generic;
using XFrame.Core;
using XFrame.Modules.Resource;
using UnityXFrame.Core;

namespace UnityXFrameLib.Localize
{
    public static class LocalizeExt
    {
        private static Dictionary<int, XEventHandler> m_LangChangeHandlers = new Dictionary<int, XEventHandler>();

        public static void Register(TextMeshProUGUI textCom, Action<TextMeshProUGUI> handler)
        {
            int code = textCom.GetHashCode();
            if (m_LangChangeHandlers.ContainsKey(code))
                Unregister(textCom);

            XEventHandler langChangeHandler = (e) =>
            {
                textCom.font = LoadFont(Global.I18N.Lang, Language.English);
                handler?.Invoke(textCom);
            };
            Global.I18N.Event.Listen(LanguageChangeEvent.EventId, langChangeHandler);
            m_LangChangeHandlers.Add(code, langChangeHandler);
            langChangeHandler?.Invoke(null);
        }

        public static void Unregister(TextMeshProUGUI textCom)
        {
            int code = textCom.GetHashCode();
            if (m_LangChangeHandlers.TryGetValue(code, out XEventHandler handler))
            {
                Global.I18N.Event.Unlisten(LanguageChangeEvent.EventId, handler);
                m_LangChangeHandlers.Remove(code);
            }
        }

        public static TMP_FontAsset LoadFont(Language lang)
        {
            return Global.LocalRes.Load<TMP_FontAsset>($"Fonts & Materials/{lang}");
        }

        public static TMP_FontAsset LoadFont(Language lang, Language defaultLang)
        {
            TMP_FontAsset font = LoadFont(lang);
            if (font == null)
                font = LoadFont(defaultLang);
            return font;
        }

        public static void SetLocal(this TextMeshProUGUI textCom, Language language, int key, params object[] values)
        {
            textCom.text = Global.I18N.GetValue(language, key, values);
        }

        public static void SetLocal(this TextMeshProUGUI textCom, int key, params object[] values)
        {
            textCom.text = Global.I18N.GetValue(key, values);
        }

        public static void SetLocal(this TextMeshProUGUI textCom, Language language, LanguageParam param)
        {
            textCom.text = Global.I18N.GetValue(language, param);
        }

        public static void SetLocal(this TextMeshProUGUI textCom, LanguageParam param)
        {
            textCom.text = Global.I18N.GetValue(param);
        }

        public static void SetLocalParam(this TextMeshProUGUI textCom, int key, params int[] args)
        {
            textCom.text = Global.I18N.GetValueParam(key, args);
        }

        public static void SetLocalParam(this TextMeshProUGUI textCom, Language language, int key, params int[] args)
        {
            textCom.text = Global.I18N.GetValueParam(language, key, args);
        }

        public static void SetLocalParam(this TextMeshProUGUI textCom, LanguageIdParam param)
        {
            textCom.text = Global.I18N.GetValueParam(param);
        }

        public static void SetLocalParam(this TextMeshProUGUI textCom, Language language, LanguageIdParam param)
        {
            textCom.text = Global.I18N.GetValueParam(language, param);
        }
    }
}

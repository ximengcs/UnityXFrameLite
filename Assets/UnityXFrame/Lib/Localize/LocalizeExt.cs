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
                textCom.font = LoadFont(LocalizeModule.Inst.Lang, Language.English);
                handler?.Invoke(textCom);
            };
            LocalizeModule.Inst.Event.Listen(LanguageChangeEvent.EventId, langChangeHandler);
            m_LangChangeHandlers.Add(code, langChangeHandler);
            langChangeHandler?.Invoke(null);
        }

        public static void Unregister(TextMeshProUGUI textCom)
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
            return Entry.GetModule<ResModule>(Constant.LOCAL_RES_MODULE).Load<TMP_FontAsset>($"Fonts & Materials/{lang}");
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
            textCom.text = LocalizeModule.Inst.GetValue(language, key, values);
        }

        public static void SetLocal(this TextMeshProUGUI textCom, int key, params object[] values)
        {
            textCom.text = LocalizeModule.Inst.GetValue(key, values);
        }

        public static void SetLocal(this TextMeshProUGUI textCom, Language language, LanguageParam param)
        {
            textCom.text = LocalizeModule.Inst.GetValue(language, param);
        }

        public static void SetLocal(this TextMeshProUGUI textCom, LanguageParam param)
        {
            textCom.text = LocalizeModule.Inst.GetValue(param);
        }

        public static void SetLocalParam(this TextMeshProUGUI textCom, int key, params int[] args)
        {
            textCom.text = LocalizeModule.Inst.GetValueParam(key, args);
        }

        public static void SetLocalParam(this TextMeshProUGUI textCom, Language language, int key, params int[] args)
        {
            textCom.text = LocalizeModule.Inst.GetValueParam(language, key, args);
        }

        public static void SetLocalParam(this TextMeshProUGUI textCom, LanguageIdParam param)
        {
            textCom.text = LocalizeModule.Inst.GetValueParam(param);
        }

        public static void SetLocalParam(this TextMeshProUGUI textCom, Language language, LanguageIdParam param)
        {
            textCom.text = LocalizeModule.Inst.GetValueParam(language, param);
        }
    }
}

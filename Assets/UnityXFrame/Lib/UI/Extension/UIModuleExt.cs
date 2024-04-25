using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XFrame.Modules.Tasks;
using System.Collections.Generic;
using UnityXFrame.Core.UIElements;
using XFrame.Modules.Reflection;
using UnityXFrame.Core;
using XFrame.Tasks;

namespace UnityXFrameLib.UIElements
{
    public static class UIModuleExt
    {
        public static void AddButtonListener(this IUI ui, string buttonName, UnityAction callback)
        {
            ui.GetCom<UIFinder>().GetUI<Button>(buttonName).onClick.AddListener(callback);
        }

        public static void RemoveButtonListener(this IUI ui, string buttonName, UnityAction callback)
        {
            ui.GetCom<UIFinder>().GetUI<Button>(buttonName).onClick.RemoveListener(callback);
        }

        public static void RemoveButtonListeners(this IUI ui, string buttonName)
        {
            ui.GetCom<UIFinder>().GetUI<Button>(buttonName).onClick.RemoveAllListeners();
        }

        public static Tween DOAnchoredPos(this RectTransform tf, Vector2 target, float duration)
        {
            return DOTween.To(() => tf.anchoredPosition, (pos) => tf.anchoredPosition = pos, target, duration);
        }

        public static Tween DOAlpha(this CanvasGroup group, float target, float duration)
        {
            return DOTween.To(() => group.alpha, (pos) => group.alpha = pos, target, duration);
        }

        public static async XTask CollectAutoTask()
        {
            await InnerFactoryTask(Global.Type.GetOrNewWithAttr<AutoLoadUIAttribute>(), Global.UI.PreloadResource);
            await InnerFactorySpwnTask(Global.Type.GetOrNewWithAttr<AutoSpwanUIAttribute>(), Global.UI.Spwan);
        }

        private static async XTask InnerFactoryTask(TypeSystem typeSys, Func<Type, int, XTask> handler)
        {
            foreach (Type type in typeSys)
            {
                UIAutoAttribute attr = Global.Type.GetAttribute<UIAutoAttribute>(type);
                await handler(type, attr.UseResModule);
            }
        }

        private static async XTask InnerFactorySpwnTask(TypeSystem typeSys, Func<Type, int, XTask> handler)
        {
            foreach (Type type in typeSys)
            {
                AutoSpwanUIAttribute attr = Global.Type.GetAttribute<AutoSpwanUIAttribute>(type);
                for (int i = 0; i < attr.Count; i++)
                    await handler(type, attr.UseResModule);
            }
        }
    }
}

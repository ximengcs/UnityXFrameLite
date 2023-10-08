using System;
using DG.Tweening;
using UnityEngine;
using UnityXFrame.Core.UIElements;
using XFrame.Modules.Tasks;
using XFrame.Modules.XType;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using XFrame.Core;

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

        public static ITask CollectAutoTask()
        {
            XTask task = XModule.Task.GetOrNew<XTask>();
            task.Add(InnerFactoryTask(XModule.Type.GetOrNewWithAttr<AutoLoadUIAttribute>(), XModule.UI.PreloadResource));
            task.Add(InnerFactoryTask(XModule.Type.GetOrNewWithAttr<AutoSpwanUIAttribute>(), XModule.UI.Spwan));
            return task;
        }

        private static ITask InnerFactoryTask(TypeSystem typeSys, Func<IEnumerable<Type>, int, ITask> handler)
        {
            XTask task = XModule.Task.GetOrNew<XTask>();
            Dictionary<int, List<Type>> map = new Dictionary<int, List<Type>>();
            foreach (Type type in typeSys)
            {
                UIAutoAttribute attr = XModule.Type.GetAttribute<UIAutoAttribute>(type);
                if (!map.TryGetValue(attr.UseResModule, out List<Type> list))
                {
                    list = new List<Type>(typeSys.Count);
                    map.Add(attr.UseResModule, list);
                }
                list.Add(type);
            }

            foreach (var entry in map)
                task.Add(handler(entry.Value, entry.Key));
            return task;
        }
    }
}

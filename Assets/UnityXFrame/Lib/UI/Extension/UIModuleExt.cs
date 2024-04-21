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

        public static List<ITask> CollectAllAutoTask()
        {
            List<ITask> list = new List<ITask>(16);
            InnerCollectInfo(Global.Type.GetOrNewWithAttr<AutoLoadUIAttribute>(), Global.UI.PreloadResource, list);
            InnerCollectInfo(Global.Type.GetOrNewWithAttr<AutoSpwanUIAttribute>(), Global.UI.Spwan, list);
            return list;
        }

        public static async XTask CollectAutoTask()
        {
            await InnerFactoryTask(Global.Type.GetOrNewWithAttr<AutoLoadUIAttribute>(), Global.UI.PreloadResource);
            await InnerFactoryTask(Global.Type.GetOrNewWithAttr<AutoSpwanUIAttribute>(), Global.UI.Spwan);
        }

        private static async XTask InnerFactoryTask(TypeSystem typeSys, Func<IEnumerable<Type>, int, ITask> handler)
        {
            Dictionary<int, List<Type>> map = new Dictionary<int, List<Type>>();
            foreach (Type type in typeSys)
            {
                UIAutoAttribute attr = Global.Type.GetAttribute<UIAutoAttribute>(type);
                if (!map.TryGetValue(attr.UseResModule, out List<Type> list))
                {
                    list = new List<Type>(typeSys.Count);
                    map.Add(attr.UseResModule, list);
                }
                list.Add(type);
            }

            foreach (var entry in map)
            {
                handler(entry.Value, entry.Key);
                await XTask.NextFrame();
            }
        }

        private static void InnerCollectInfo(TypeSystem typeSys, Func<Type, int, ITask> handler, List<ITask> result)
        {
            foreach (Type type in typeSys)
            {
                UIAutoAttribute attr = Global.Type.GetAttribute<UIAutoAttribute>(type);
                if (attr != null)
                {
                    ITask task = handler(type, attr.UseResModule);
                    result.Add(task);
                }
            }
        }
    }
}

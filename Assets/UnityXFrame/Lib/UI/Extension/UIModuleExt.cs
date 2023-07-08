using System;
using DG.Tweening;
using UnityEngine;
using UnityXFrame.Core.UIs;
using XFrame.Modules.Tasks;
using XFrame.Modules.XType;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityXFrameLib.UI
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
            XTask task = TaskModule.Inst.GetOrNew<XTask>();
            task.Add(InnerFactoryTask(TypeModule.Inst.GetOrNewWithAttr<AutoLoadUIAttribute>(), UIModule.Inst.PreloadResource));
            task.Add(InnerFactoryTask(TypeModule.Inst.GetOrNewWithAttr<AutoSpwanUIAttribute>(), UIModule.Inst.Spwan));
            return task;
        }

        private static ITask InnerFactoryTask(TypeSystem typeSys, Func<IEnumerable<Type>, bool, ITask> handler)
        {
            XTask task = TaskModule.Inst.GetOrNew<XTask>();
            List<Type> navtive = new List<Type>(typeSys.Count);
            List<Type> nonNative = new List<Type>(typeSys.Count);
            InnerCollect(typeSys, navtive, nonNative);
            task.Add(handler(navtive, true));
            task.Add(handler(nonNative, false));
            return task;
        }

        private static void InnerCollect(TypeSystem typeSys, List<Type> nativeList, List<Type> nonNativeSys)
        {
            foreach (Type type in typeSys)
            {
                UIAutoAttribute attr = TypeModule.Inst.GetAttribute<UIAutoAttribute>(type);
                if (attr.Native)
                    nativeList.Add(type);
                else
                    nonNativeSys.Add(type);
            }
        }
    }
}

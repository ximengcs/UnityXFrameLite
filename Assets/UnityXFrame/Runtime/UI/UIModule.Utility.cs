﻿using System;
using UnityEngine;

namespace UnityXFrame.Core.UIElements
{
    public partial class UIModule
    {
        internal static int SetLayer(Transform root, IUIElement element, int layer, Action<Transform, int> onIndexChange)
        {
            if (root.childCount == 0)
                return 0;
            layer = Mathf.Clamp(layer, 0, root.childCount - 1);
            Transform check = root.GetChild(layer);
            if (check.name == element.Name)
                return layer;

            bool find = false;
            int curIndex = 0;
            Transform[] list = new Transform[root.childCount];
            for (int i = 0; i < list.Length; i++, curIndex++)
            {
                Transform child = root.GetChild(i);
                if (!find && child.name == element.Name)
                {
                    find = true;
                    list[layer] = child;
                    if (layer != curIndex)
                        curIndex--;
                }
                else
                {
                    if (layer == curIndex)
                        curIndex++;
                    list[curIndex] = child;
                }
            }

            for (int i = 0; i < list.Length; i++)
            {
                Transform child = list[i];
                if (child.GetSiblingIndex() != i)
                {
                    child.SetSiblingIndex(i);
                    onIndexChange(child, i);
                }
            }
            return layer;
        }
    }
}

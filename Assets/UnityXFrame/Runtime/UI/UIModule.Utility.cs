using UnityEngine;

namespace UnityXFrame.Core.UIs
{
    public partial class UIModule
    {
        internal static string GetInstName(IUIElement ui)
        {
            return $"{ui.GetType().Name}{ui.GetHashCode()}";
        }

        internal static void SetLayer(Transform root, IUIElement element, int layer)
        {
            Transform check = root.GetChild(layer);
            if (check.name == element.Name)
                return;
            Debug.LogWarning(check.name + " " + element.Name);

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
                    child.SetSiblingIndex(i);
            }
        }
    }
}

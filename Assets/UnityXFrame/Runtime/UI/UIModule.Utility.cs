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
            if (check.name == GetInstName(element))
                return;

            bool find = false;
            Transform[] list = new Transform[root.childCount];
            Vector3[] positions = new Vector3[root.childCount];

            int curIndex = 0;
            for (int i = 0; i < list.Length; i++, curIndex++)
            {
                Transform child = root.GetChild(i);
                if (!find && child.name == element.Name)
                {
                    find = true;
                    list[layer] = child;
                    positions[layer] = child.position;
                    if (layer != curIndex)
                        curIndex--;
                }
                else
                {
                    if (layer == curIndex)
                        curIndex++;
                    list[curIndex] = child;
                    positions[curIndex] = child.position;
                }
            }

            root.DetachChildren();
            for (int i = 0; i < list.Length; i++)
            {
                Transform child = list[i];
                child.SetParent(root);
                child.position = positions[i];
            }
        }
    }
}

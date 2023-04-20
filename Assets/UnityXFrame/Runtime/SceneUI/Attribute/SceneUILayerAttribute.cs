using System;

namespace UnityXFrame.Core.SceneUIs
{
    public class SceneUILayer : Attribute
    {
        public int Layer;

        public SceneUILayer(int layer)
        {
            Layer = layer;
        }
    }
}

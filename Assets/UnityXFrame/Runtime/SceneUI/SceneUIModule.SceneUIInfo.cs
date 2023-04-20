using UnityEngine;

namespace UnityXFrame.Core.SceneUIs
{
    public partial class SceneUIModule
    {
        private struct SceneUIInfo
        {
            public ISceneUI Inst;
            public GameObject UnityInst;
            public int Id;

            public SceneUIInfo(ISceneUI inst, GameObject unityInst)
            {
                Inst = inst;
                UnityInst = unityInst;
                Id = inst.GetHashCode();
            }
        }
    }
}

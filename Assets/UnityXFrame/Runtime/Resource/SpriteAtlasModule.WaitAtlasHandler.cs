
using UnityEngine.U2D;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public partial class SpriteAtlasModule
    {
        private class WaitAtlasHandler : IResHandler
        {
            private ResLoadTask<SpriteAtlas> m_AtlasTask;
            private string m_SpriteName;

            public object Data => m_AtlasTask.Res.GetSprite(m_SpriteName);

            public bool IsDone => m_AtlasTask.IsComplete;

            public float Pro => m_AtlasTask.Pro;

            public WaitAtlasHandler(ResLoadTask<SpriteAtlas> atlasTask, string spriteName)
            {
                m_SpriteName = spriteName;
                m_AtlasTask = atlasTask;
            }

            public void Start()
            {
                m_AtlasTask.Start();
            }

            public void Dispose()
            {
                m_SpriteName = null;
            }
        }
    }
}

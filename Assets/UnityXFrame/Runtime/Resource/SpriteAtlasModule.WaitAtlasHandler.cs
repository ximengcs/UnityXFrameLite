
using System;
using UnityEngine;
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
            private Type m_Type;

            public object Data
            {
                get
                {
                    SpriteAtlas atlas = m_AtlasTask.GetResult();
                    if (atlas == null)
                    {
                        throw new NullReferenceException($"SpriteAtlasModule Handler Error, Name : {m_SpriteName}");
                    }
                    return atlas.GetSprite(m_SpriteName);
                }
            }

            public string AssetPath => m_SpriteName;

            public Type AssetType => m_Type;

            public bool IsDone => m_AtlasTask.IsCompleted;

            public double Pro => m_AtlasTask.Progress;

            public WaitAtlasHandler(ResLoadTask<SpriteAtlas> atlasTask, string spriteName, Type type)
            {
                m_Type = type;
                m_SpriteName = spriteName;
                m_AtlasTask = atlasTask;
            }

            public void OnCancel()
            {
                m_SpriteName = null;
            }

            public void Start()
            {
                m_AtlasTask.Coroutine();
            }

            public void Dispose()
            {
                m_SpriteName = null;
            }
        }
    }
}

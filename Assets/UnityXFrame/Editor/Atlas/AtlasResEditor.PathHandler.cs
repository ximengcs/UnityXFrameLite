
namespace UnityXFrame.Editor
{
    public partial class AtlasResEditor
    {
        public interface IAtlasResHandler
        {
            void OnSavePath(string resPath, string atlasPath, out string newResPath, out string newAtlasPath);
        }
    }
}

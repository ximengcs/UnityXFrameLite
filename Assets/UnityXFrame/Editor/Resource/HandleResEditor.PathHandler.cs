
namespace UnityXFrame.Editor
{
    public partial class HandleResEditor
    {
        public interface IAtlasResHandler
        {
            void OnSavePath(string resPath, string atlasPath, out string newResPath, out string newAtlasPath);
        }
    }
}

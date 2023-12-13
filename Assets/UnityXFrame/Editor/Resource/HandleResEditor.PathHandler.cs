
namespace UnityXFrame.Editor
{
    public partial class HandleResEditor
    {
        public interface IAtlasResHandler
        {
            void OnSaveAtlasPath(string resPath, string atlasPath, out string newResPath, out string newAtlasPath);
            void OnSaveAliasPath(string rootPath, string originPath, out string newPath);
        }
    }
}

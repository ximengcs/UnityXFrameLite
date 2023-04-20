using System.Collections.Generic;

namespace UnityXFrame.Core.Resource
{
    public partial class AssetBundleResHelper
    {
        public class FileLoadInfo
        {
            public string MainName;
            public Dictionary<string, string> FileToABMap;

            public FileLoadInfo()
            {
                FileToABMap = new Dictionary<string, string>();
            }

            public void Add(string filePath, string abName)
            {
                FileToABMap.Add(filePath, abName);
            }
        }
    }
}

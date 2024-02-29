using System.IO;
using UnityEngine;
using XFrame.Modules.Archives;

namespace UnityXFrame.Runtime.Archieves
{
    public class UnityPlayerprefsArchiveUtility : IArchiveUtilityHelper
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public string ReadAllText(string path)
        {
            return PlayerPrefs.GetString(path);
        }

        public void WriteAllBytes(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }

        public void WriteAllText(string path, string data)
        {
            PlayerPrefs.SetString(path, data);
        }
    }
}

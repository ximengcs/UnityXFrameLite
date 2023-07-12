
using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class UsefulToolData : ScriptableObject
    {
        public BuildTarget CurrentBuildTarget;
        public bool IsRelease;
        public string XFrameProjectPath;
        public string XFramePath;
        public string BuildDllPath;
        public string ToProjectDllPath;
    }
}

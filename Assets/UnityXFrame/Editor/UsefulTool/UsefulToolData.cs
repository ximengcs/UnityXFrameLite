using System.Collections.Generic;
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
        public string XFrameSharePath;
        public string BuildDllPath;
        public List<string> ToProjectDllPath;
    }
}

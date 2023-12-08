using UnityEngine;
using System.Collections.Generic;
using System;

namespace UnityXFrame.Editor
{
    public class HandleResData : ScriptableObject
    {
        public List<string> ExcludeList;
        public List<ResAliasGroupData> IncludeGroupList;
    }

    [Serializable]
    public class ResAliasGroupData
    {
        public int GroupId;
        public List<string> IncludeList;
    }
}

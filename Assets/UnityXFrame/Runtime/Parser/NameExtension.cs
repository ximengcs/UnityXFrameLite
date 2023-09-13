
using System.Collections.Generic;

namespace UnityXFrame.Core.Parser
{
    public static class NameExtension
    {
        public static bool ContainsName<V>(this Dictionary<Name, V> map, Name value)
        {
            bool result = map.ContainsKey(value);
            value.Release();
            return result;
        }
    }
}

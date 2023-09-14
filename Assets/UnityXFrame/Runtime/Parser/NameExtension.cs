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

        public static V Get<V>(this Dictionary<Name, V> map, Name value, bool releaseKey = true)
        {
            V result = map[value];
            if (releaseKey)
                value.Release();
            return result;
        }

        public static bool TryGet<V>(this Dictionary<Name, V> map, Name value, out V result, bool releaseKey = true)
        {
            bool success = map.TryGetValue(value, out result);
            if (releaseKey)
                value.Release();
            return success;
        }
    }
}

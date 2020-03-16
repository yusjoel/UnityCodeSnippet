using System.Collections.Generic;

namespace Extensions
{
    public static class DictionaryExtension
    {
        public static T SafelyGet<T>(this Dictionary<string, T> dictionary, string key)
        {
            if (dictionary == null) return default(T);
            if (!dictionary.ContainsKey(key)) return default(T);
            return dictionary[key];
        }

        public static T SafelyGet<T>(this Dictionary<string, object> dictionary, string key)
        {
            if (dictionary == null) return default(T);
            if (!dictionary.ContainsKey(key)) return default(T);
            var value = dictionary[key];
            return value is T ? (T)dictionary[key] : default(T);
        }
    }
}

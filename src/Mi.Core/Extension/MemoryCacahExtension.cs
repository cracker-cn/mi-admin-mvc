using System.Collections;
using System.Reflection;

using Microsoft.Extensions.Caching.Memory;

namespace Mi.Core.Extension
{
    public static class MemoryCacahExtension
    {
        /// <summary>
        /// 获取所有缓存建
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static List<string> GetCacheKeys(this MemoryCache cache)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var fields = cache.GetType().GetField("_entries", flags);
            if(fields == null) return new List<string>();
            var entries = fields.GetValue(cache);
            var keys = new List<string>();
            if (entries is not IDictionary cacheItems) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                var key = cacheItem.Key.ToString();
                if(!string.IsNullOrEmpty(key) ) keys.Add(key);
            }
            return keys;
        }
    }
}
using System;
using System.Linq;
using System.Runtime.Caching;
using PoolHockeyBLL.Contracts;

namespace PoolHockeyBLL
{
    public class Caching : ICaching
    {
        // Gets a reference to the default MemoryCache instance. 
        private readonly ObjectCache _cache = MemoryCache.Default;
        private CacheItemPolicy _policy = null;

        public static void ClearAllCaches()
        {
            var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (var cacheKey in cacheKeys)
                MemoryCache.Default.Remove(cacheKey);
        }

        public void AddToCache(string cacheKeyName, object cacheItem)
        {
            _policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddHours(22.0) };
            // Add inside cache 
            _cache.Add(cacheKeyName, cacheItem, _policy);
        }

        public object GetCachedItem(string cacheKeyName)
        {
            return _cache.Get(cacheKeyName);
        }
    }
}

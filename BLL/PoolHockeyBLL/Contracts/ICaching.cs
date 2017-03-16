namespace PoolHockeyBLL.Contracts
{
    public interface ICaching
    {
        void AddToCache(string cacheKeyName, object cacheItem);
        object GetCachedItem(string cacheKeyName);
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.Services
{
    /// <summary>
    /// Caching service used for InMemory caching with defined expire time in minutes
    /// </summary>
    public class MemoryCacheService : ICachingService
    {
        private const int DEFAULT_CACHE_TIME_IN_SECOND = 3600;
        private readonly int cacheTimeInSeconds;
        private static IMemoryCache memoryCache;

        public MemoryCacheService(int? cacheTimeInSeconds = DEFAULT_CACHE_TIME_IN_SECOND,
            IMemoryCache cache = null,
            IOptions<MemoryCacheOptions> cacheOptions = null)
        {
            this.cacheTimeInSeconds = cacheTimeInSeconds.HasValue ? cacheTimeInSeconds.Value : DEFAULT_CACHE_TIME_IN_SECOND;
            this.ConfigureCache(cache, cacheOptions);
        }

        private void ConfigureCache(IMemoryCache cache, IOptions<MemoryCacheOptions> cacheOptions)
        {
            if (memoryCache != null)
            {
                return;
            }

            if (cache != null)
            {
                memoryCache = cache;
            }
            else
            {
                memoryCache = new MemoryCache(cacheOptions ?? new MemoryCacheOptions());
            }
        }

        /// <inheritdoc />
        public async Task<T> GetObjectFromCacheAsync<T>(string cacheItemKey, Func<Task<T>> getDataToCacheFunction, bool shouldRefresh = false)
        {
            var cachedObject = memoryCache.Get<T>(cacheItemKey);

            if (cachedObject == null || shouldRefresh)
            {
                cachedObject = await getDataToCacheFunction();
                memoryCache.Set<T>(cacheItemKey, cachedObject, TimeSpan.FromSeconds(cacheTimeInSeconds));
            }

            return cachedObject;
        }

        public async Task<T> GetObjectFromCacheAsync<T>(string cacheItemKey, Func<Task<T>> getDataToCacheFunction, TimeSpan absoluteExpiration, bool shouldRefresh = false)
        {
            var cachedObject = memoryCache.Get<T>(cacheItemKey);

            if (cachedObject == null || shouldRefresh)
            {
                cachedObject = await getDataToCacheFunction();
                memoryCache.Set<T>(cacheItemKey, cachedObject, absoluteExpiration);
            }

            return cachedObject;
        }

        public void RemoveObjectFromCache(string cacheItemKey)
        {
            memoryCache.Remove(cacheItemKey);
        }
    }
}

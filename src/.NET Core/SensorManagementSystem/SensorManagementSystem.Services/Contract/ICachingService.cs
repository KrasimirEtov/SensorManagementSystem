using System;
using System.Threading.Tasks;

namespace SensorManagementSystem.Services.Contract
{
    public interface ICachingService
    {
        /// <summary>
        /// Gets object from cache if the key is present, otherwise calls an asynchronous method that returns the object that needs to be cached
        /// </summary>
        /// <typeparam name="T">Generic parameter that will be cached</typeparam>
        /// <param name="cacheItemKey">Key for the dictionary containing cached objects</param>
        /// <param name="getDataToCacheFunction">Asynchronous method which returns the parameter/s that need to be cached</param>
        /// <param name="shouldRefresh">Boolean flag if we need to force clear and refresh the cache</param>
        /// <returns>Returns the cached object</returns>
        Task<T> GetObjectFromCacheAsync<T>(string cacheItemKey, Func<Task<T>> getDataToCacheFunction, bool shouldRefresh = false);

        /// <summary>
        /// Gets object from cache if the key is present, otherwise calls an asynchronous method that returns the object that needs to be cached
        /// </summary>
        /// <typeparam name="T">Generic parameter that will be cached</typeparam>
        /// <param name="cacheItemKey">Key for the dictionary containing cached objects</param>
        /// <param name="getDataToCacheFunction">Asynchronous method which returns the parameter/s that need to be cached</param>
        /// <param name="absoluteExpiration">Time to set when the cache will expire</param>
        /// <param name="shouldRefresh">Boolean flag if we need to force clear and refresh the cache</param>
        /// <returns>Returns the cached object</returns>
        Task<T> GetObjectFromCacheAsync<T>(string cacheItemKey, Func<Task<T>> getDataToCacheFunction, TimeSpan absoluteExpiration, bool shouldRefresh = false);

        /// <summary>
        /// Remove object from cache by key
        /// </summary>
        /// <param name="cacheItemKey"></param>
        void RemoveObjectFromCache(string cacheItemKey);
    }
}

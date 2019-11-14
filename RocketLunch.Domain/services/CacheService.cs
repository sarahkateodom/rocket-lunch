using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RocketLunch.domain.contracts;

namespace RocketLunch.domain.services
{
    public class CacheService : ICache
    {
        private IDistributedCache cache;
        public CacheService(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public async Task<T> GetAsync<T>(string key)
        {
            string value = await this.cache.GetStringAsync(key);
            return (value != null) ? JsonConvert.DeserializeObject<T>(value) : default(T);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationSpan = null)
        {
            expirationSpan = expirationSpan ?? new TimeSpan(0, 5, 0);
            await this.cache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationSpan
            });
        }

        public async Task ClearKey(string key)
        {
            await this.cache.RemoveAsync(key);
        }
    }
}
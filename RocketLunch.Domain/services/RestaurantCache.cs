using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using System.Linq;

namespace RocketLunch.domain.services
{
    public class RestaurantCache : IRestaurantCache
    {
        private ICache cache;
        private string seenOptionsSuffix = "_seenoptions";
        private string sessionSuffix = "_usersessions";
        private string sessionSearchSuffix = "_sessionsearch";
        private string sessionSearchOptions = "_sessionsearchoptions";
        public RestaurantCache(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<List<string>> GetSeenOptionsAsync(Guid sessionId)
        {
            return await cache.GetAsync<List<string>>($"{sessionId.ToString()}{this.seenOptionsSuffix}") ?? new List<string>();
        }

        public async Task AddSeenOptionAsync(Guid sessionId, string option)
        {
            List<string> options = (await cache.GetAsync<List<String>>($"{sessionId.ToString()}{seenOptionsSuffix}")) ?? new List<string>();
            options.Add(option);
            await cache.SetAsync($"{sessionId.ToString()}{seenOptionsSuffix}", options);
        }

        public async Task<List<int>> GetUserSessionAsync(Guid sessionId)
        {
            return await this.cache.GetAsync<List<int>>($"{sessionId.ToString()}{this.sessionSuffix}");
        }

        public async Task SetUserSessionAsync(Guid sessionId, List<int> userIds)
        {
            await cache.SetAsync($"{sessionId.ToString()}{sessionSuffix}", userIds);
        }

        public async Task<List<RestaurantDto>> GetRestaurantListAsync(string zip)
        {
            return await cache.GetAsync<List<RestaurantDto>>(zip);
        }

        public async Task SetRestaurantListAsync(string zip, List<RestaurantDto> restaurants)
        {
            await this.cache.SetAsync(zip, restaurants, new TimeSpan(24, 0, 0));
        }

        public async Task SetSessionSearchOptionsAsync(Guid sessionId, SearchOptions options)
        {
            await cache.SetAsync($"{sessionId.ToString()}{sessionSearchOptions}", options);
        }

        public async Task<SearchOptions> GetSessionSearchOptionsAsync(Guid sessionId)
        {
            return await cache.GetAsync<SearchOptions>($"{sessionId.ToString()}{sessionSearchOptions}");
        }

        public async Task ClearSessionSearchAsync(Guid sessionId)
        {
            await cache.ClearKey($"{sessionId.ToString()}{sessionSearchSuffix}");
        }

    }
}

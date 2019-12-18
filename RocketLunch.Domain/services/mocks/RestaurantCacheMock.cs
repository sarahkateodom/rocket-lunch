using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.services.mocks
{
    public class RestaurantCacheMock : IRestaurantCache
    {
        private List<string> seenOptions = new List<string>();
        public async Task AddSeenOptionAsync(Guid sessionId, string option)
        {
            await Task.Delay(1);
            seenOptions.Add(option);
        }
        public async Task<List<string>> GetSeenOptionsAsync(Guid sessionId)
        {
            return await Task.FromResult(seenOptions);
        }

        private List<RestaurantDto> restaurants = null;
        public async Task<List<RestaurantDto>> GetRestaurantListAsync(string zip)
        {
            return await Task.FromResult(restaurants);
        }

        public async Task SetRestaurantListAsync(string zip, List<RestaurantDto> restaurants)
        {
            await Task.Delay(0);
            this.restaurants = restaurants;
        }

        private List<int> userSession = new List<int>();
        public async Task<List<int>> GetUserSessionAsync(Guid sessionId)
        {
            return await Task.FromResult(userSession);
        }

        public async Task SetUserSessionAsync(Guid sessionId, List<int> userIds)
        {
            await Task.Delay(0);
            userSession = userIds;
        }

        private SearchOptions userSessionSearchOptions;
        public async Task SetSessionSearchOptionsAsync(Guid sessionId, SearchOptions options)
        {
            await Task.Delay(0);
            userSessionSearchOptions = options;
        }

        public async Task<SearchOptions> GetSessionSearchOptionsAsync(Guid sessionId)
        {
            return await Task.FromResult(userSessionSearchOptions);
        }

        public async Task ClearSessionSearchAsync(Guid sessionId)
        {
            await Task.Delay(0);
            restaurants = null;
        }
    }
}
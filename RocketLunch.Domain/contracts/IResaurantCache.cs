using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.contracts
{
    public interface IRestaurantCache
    {
        Task<List<RestaurantDto>> GetRestaurantListAsync(Guid sessionId);
        Task SetRestaurantListAsync(Guid sessionId, List<RestaurantDto> restaurants);
        Task AddSeenOptionAsync(Guid sessionId, string option);
        Task<List<string>> GetSeenOptionsAsync(Guid sessionId);
        Task SetUserSessionAsync(Guid sessionId, List<int> userIds);
        Task<List<int>> GetUserSessionAsync(Guid sessionId);
        Task SetSessionSearchOptionsAsync(Guid sessionId, SearchOptions options);
        Task<SearchOptions> GetSessionSearchOptionsAsync(Guid sessionId);
        Task ClearSessionSearchAsync(Guid sessionId);
    }
}
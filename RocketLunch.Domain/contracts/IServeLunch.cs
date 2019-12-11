using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;
using RocketLunch.domain.utilities;

namespace RocketLunch.domain.contracts
{
    public interface IServeLunch
    {
        Task<RestaurantDto> GetRestaurantAsync(Guid sessionId, SearchOptions options);
        Task<IEnumerable<RestaurantDto>> GetRestaurantsAsync(string zip);
    }
}
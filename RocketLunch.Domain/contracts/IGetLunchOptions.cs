using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.contracts
{
    public interface IGetLunchOptions
    {
        Task<IEnumerable<RestaurantDto>> GetAvailableRestaurantOptionsAsync();
    }
}
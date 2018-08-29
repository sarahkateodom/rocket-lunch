using System.Collections.Generic;
using System.Threading.Tasks;
using makelunch.domain.dtos;

namespace makelunch.domain.contracts
{
    public interface IGetLunchOptions
    {
        Task<IEnumerable<RestaurantDto>> GetAvailableRestaurantOptionsAsync();
    }
}
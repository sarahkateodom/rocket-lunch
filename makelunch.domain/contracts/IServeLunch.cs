using System;
using System.Threading.Tasks;
using makelunch.domain.dtos;
using makeLunch.domain.utilities;

namespace makelunch.domain.contracts
{
    public interface IServeLunch
    {
        Task<Either<HttpStatusCodeErrorResponse, RestaurantDto>> GetRestaurantAsync(Guid sessionId);
    }
}
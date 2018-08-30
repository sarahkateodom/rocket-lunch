using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using makeLunch.domain.utilities;

namespace makelunch.domain.services
{
    public class LunchService : IServeLunch
    {
        private IGetLunchOptions _lunchOptions;
        private Random _random;

        public LunchService(IGetLunchOptions lunchOptions)
        {
            _lunchOptions = lunchOptions ?? throw new ArgumentNullException("lunchOptions");
            _random = new Random();
        }

        public async Task<Either<HttpStatusCodeErrorResponse, RestaurantDto>> GetRestaurantAsync()
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                List<RestaurantDto> restaurants = (await _lunchOptions.GetAvailableRestaurantOptionsAsync().ConfigureAwait(false)).ToList();
                return restaurants[_random.Next(restaurants.Count - 1)];
            }).ConfigureAwait(false);
        }
    }
}
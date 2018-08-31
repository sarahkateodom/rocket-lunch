using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using makeLunch.domain.utilities;
using makelunch.domain.exceptions;

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

        public async Task<Either<HttpStatusCodeErrorResponse, RestaurantDto>> GetRestaurantAsync(Guid sessionId)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                List<RestaurantDto> restaurants = (await _lunchOptions.GetAvailableRestaurantOptionsAsync().ConfigureAwait(false)).ToList();
                List<string> seenOptions = RestaurantCash.GetSeenOptions(sessionId.ToString());
                RestaurantDto result;
                if (seenOptions == null) 
                {
                    result = restaurants[_random.Next(restaurants.Count - 1)];
                }
                else
                {
                    var filtered = restaurants.Where(x => !seenOptions.Contains(x.Name)).ToList();
                    if (!filtered.Any()) throw new TooManyRequestsException();
                    result = filtered[_random.Next(filtered.Count - 1)];
                }

                RestaurantCash.AddSeenOption(sessionId.ToString(), result.Name);
                return result;
            }).ConfigureAwait(false);
        }

        public async Task<Either<HttpStatusCodeErrorResponse, IEnumerable<RestaurantDto>>> GetRestaurantsAsync()
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                return await _lunchOptions.GetAvailableRestaurantOptionsAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
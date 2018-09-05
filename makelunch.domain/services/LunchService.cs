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
        private IChaos _random;
        private IRepository _repo;

        public LunchService(IGetLunchOptions lunchOptions, IRepository repo, IChaos random)
        {
            _lunchOptions = lunchOptions ?? throw new ArgumentNullException("lunchOptions");
            _random = random ?? throw new ArgumentNullException("RandomService");
            _repo = repo ?? throw new ArgumentNullException("repo");
        }

        public async Task<Either<HttpStatusCodeErrorResponse, RestaurantDto>> GetRestaurantAsync(Guid sessionId)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                List<int> userIds = RestaurantCash.GetUserSession(sessionId.ToString());
                List<UserDto> users = (await _repo.GetUsersAsync()).Where(u => userIds.Contains(u.Id)).ToList();
                List<RestaurantDto> restaurants = (await _lunchOptions.GetAvailableRestaurantOptionsAsync().ConfigureAwait(false)).ToList();
                
                // Filter by user nopes
                List<string> nopes = users.SelectMany(u => u.Nopes).ToList();
                restaurants = restaurants.Where(r => !nopes.Contains(r.Id)).ToList();
                
                RestaurantDto result;
                // Filter by "soft" nopes
                List<string> seenOptions = RestaurantCash.GetSeenOptions(sessionId.ToString());
                if (seenOptions == null) 
                {
                    result = restaurants[_random.Next(restaurants.Count - 1)];
                }
                else
                {
                    restaurants = restaurants.Where(x => !seenOptions.Contains(x.Name)).ToList();
                    if (!restaurants.Any()) throw new TooManyRequestsException();
                    result = restaurants[_random.Next(restaurants.Count - 1)];
                }

                // Add result to "soft" nopes
                RestaurantCash.AddSeenOption(sessionId.ToString(), result.Name);

                return result;
            }).ConfigureAwait(false);
        }

        public async Task<Either<HttpStatusCodeErrorResponse, IEnumerable<RestaurantDto>>> GetRestaurantsAsync()
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                return (await _lunchOptions.GetAvailableRestaurantOptionsAsync().ConfigureAwait(false));
            }).ConfigureAwait(false);
        }
    }
}
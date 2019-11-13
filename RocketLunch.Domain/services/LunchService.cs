using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.utilities;
using RocketLunch.domain.exceptions;

namespace RocketLunch.domain.services
{
    public class LunchService : IServeLunch
    {
        private IGetLunchOptions lunchOptions;
        private IChaos random;
        private IRepository repo;
        private IRestaurantCache cache;

        public LunchService(IGetLunchOptions lunchOptions, IRepository repo, IChaos random, IRestaurantCache cache)
        {
            this.lunchOptions = lunchOptions ?? throw new ArgumentNullException("lunchOptions");
            this.random = random ?? throw new ArgumentNullException("RandomService");
            this.repo = repo ?? throw new ArgumentNullException("repo");
            this.cache = cache ?? throw new ArgumentNullException("cache");
        }

        public async Task<Either<HttpStatusCodeErrorResponse, RestaurantDto>> GetRestaurantAsync(Guid sessionId, SearchOptions options)
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                List<int> userIds = await cache.GetUserSessionAsync(sessionId);
                var sessions = await repo.GetUsersAsync();
                List<UserDto> users = sessions.Where(u => userIds.Contains(u.Id)).ToList();
                List<RestaurantDto> restaurants = (await lunchOptions.GetAvailableRestaurantOptionsAsync(sessionId, options).ConfigureAwait(false)).ToList();

                // Filter by user nopes
                List<string> nopes = users.SelectMany(u => u.Nopes).ToList();
                restaurants = restaurants.Where(r => !nopes.Contains(r.Id)).ToList();

                RestaurantDto result;
                // Filter by "soft" nopes
                List<string> seenOptions = await cache.GetSeenOptionsAsync(sessionId);
                if (seenOptions == null)
                {
                    result = restaurants[random.Next(restaurants.Count - 1)];
                }
                else
                {
                    restaurants = restaurants.Where(x => !seenOptions.Contains(x.Name)).ToList();
                    if (!restaurants.Any()) throw new TooManyRequestsException();
                    result = restaurants[random.Next(restaurants.Count - 1)];
                }

                // Add result to "soft" nopes
                await cache.AddSeenOptionAsync(sessionId, result.Name);

                return result;
            }).ConfigureAwait(false);
        }

        public async Task<Either<HttpStatusCodeErrorResponse, IEnumerable<RestaurantDto>>> GetRestaurantsAsync()
        {
            return await ExceptionHandler.HandleExceptionAsync(async () =>
            {
                return (await lunchOptions.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions()).ConfigureAwait(false));
            }).ConfigureAwait(false);
        }
    }
}
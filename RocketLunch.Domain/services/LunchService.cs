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

        public async Task<RestaurantDto> GetRestaurantAsync(Guid sessionId, SearchOptions options)
        {
            List<RestaurantDto> restaurants = (await lunchOptions.GetAvailableRestaurantOptionsAsync(sessionId, options).ConfigureAwait(false)).ToList();

            // Filter by user nopes
            IEnumerable<string> nopes = await this.repo.GetNopesAsync(options.UserIds).ConfigureAwait(false);
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
        }

        public async Task<IEnumerable<RestaurantDto>> GetRestaurantsAsync(string zip)
        {
            return (await lunchOptions.GetAllRestaurantsInZipAsync(zip).ConfigureAwait(false));
        }
    }
}
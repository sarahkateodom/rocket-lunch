using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.utilities;
using Newtonsoft.Json;
using System;

namespace RocketLunch.domain.services
{
    public class YelpService : IGetLunchOptions
    {
        private string apiKey;
        private IRestaurantCache cache;

        public YelpService(string apiKey, IRestaurantCache cache)
        {
            this.apiKey = apiKey;
            this.cache = cache;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAvailableRestaurantOptionsAsync(Guid sessionId)
        {
            List<RestaurantDto> restaurantList = await cache.GetRestaurantListAsync(sessionId);
            if (restaurantList != null) return restaurantList;
            List<RestaurantDto> businesses = new List<RestaurantDto>();
            int offset = 0;
            YelpResultDto dto;
            do
            {
                dto = await GetYelpRestaurantsAsync(offset).ConfigureAwait(false);
                businesses.AddRange(dto.Businesses);
                offset = businesses.Count;
            }
            while (businesses.Count < dto.Total);
            businesses.Add(new RestaurantDto
            {
                Name = "Be The Change",
                Id = "BTC"
            });

            List<RestaurantDto> restaurants = businesses.OrderBy(x => x.Name).ToList();
            await cache.SetRestaurantListAsync(sessionId, restaurants);
            return restaurants;
        }

        private async Task<YelpResultDto> GetYelpRestaurantsAsync(int offset)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                HttpResponseMessage message = await client.GetAsync("https://api.yelp.com/v3/businesses/search?categories=restaurants,!hotdogs&location=38655&radius=20000&limit=50&sort_by=best_match&offset=" + offset);
                return JsonConvert.DeserializeObject<YelpResultDto>(await message.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }
    }
}
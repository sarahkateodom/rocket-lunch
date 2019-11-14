using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.utilities;
using Newtonsoft.Json;
using System;
using RocketLunch.domain.enumerations;

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

        public async Task<IEnumerable<RestaurantDto>> GetAvailableRestaurantOptionsAsync(Guid sessionId, SearchOptions options)
        {
            SearchOptions cachedOptions = await cache.GetSessionSearchOptionsAsync(sessionId);
            if( cachedOptions != null && JsonConvert.SerializeObject(cachedOptions) != JsonConvert.SerializeObject(options) ) await cache.ClearSessionSearchAsync(sessionId);
            List<RestaurantDto> restaurantList = await cache.GetRestaurantListAsync(sessionId);
            if (restaurantList != null) return restaurantList;
            List<RestaurantDto> businesses = new List<RestaurantDto>();
            int offset = 0;
            YelpResultDto dto;
            do
            {
                dto = await GetYelpRestaurantsAsync(offset, options).ConfigureAwait(false);
                businesses.AddRange(dto.Businesses);
                offset = businesses.Count;
            }
            while (businesses.Count < dto.Total);

            List<RestaurantDto> restaurants = businesses.OrderBy(x => x.Name).ToList();
            await cache.SetRestaurantListAsync(sessionId, restaurants);
            //cache search options
            await cache.SetSessionSearchOptionsAsync(sessionId, options);
            return restaurants;
        }

        private async Task<YelpResultDto> GetYelpRestaurantsAsync(int offset, SearchOptions options)
        {
            string openAt = "";
            if (options.Meal != MealTime.all)
            {
                openAt = "&open_at=" + DateTime.Now.Date.AddHours(options.Meal.GetHoursFromMidnight()).GetUnixTime().ToString();
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                HttpResponseMessage message = await client.GetAsync("https://api.yelp.com/v3/businesses/search?categories=restaurants,!hotdogs&location=38655&radius=20000&limit=50&sort_by=best_match&offset=" + offset + openAt);
                return JsonConvert.DeserializeObject<YelpResultDto>(await message.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }
    }
}
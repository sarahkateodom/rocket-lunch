using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.utilities;
using Newtonsoft.Json;

namespace RocketLunch.domain.services
{
    public class YelpService : IGetLunchOptions
    {
        private string _apiKey;

        public YelpService(string apiKey)
        {
           _apiKey = apiKey;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAvailableRestaurantOptionsAsync()
        {
            List<RestaurantDto> restaurantList = RestaurantCash.RestaurantList;
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
            businesses.Add(new RestaurantDto {
                Name = "Be The Change",
                Id = "BTC"
            });

            RestaurantCash.RestaurantList = businesses.OrderBy(x => x.Name).ToList();
            return RestaurantCash.RestaurantList;
        }

        private async Task<YelpResultDto> GetYelpRestaurantsAsync(int offset)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);
                HttpResponseMessage message = await client.GetAsync("https://api.yelp.com/v3/businesses/search?categories=restaurants,!hotdogs&location=38655&radius=20000&limit=50&sort_by=best_match&offset=" + offset);
                return JsonConvert.DeserializeObject<YelpResultDto>(await message.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }
    }
}
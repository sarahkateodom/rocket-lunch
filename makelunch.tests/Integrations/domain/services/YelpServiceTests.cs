using System.Collections.Generic;
using makelunch.domain.dtos;
using makelunch.domain.services;
using Xunit;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace makelunch.tests.integrations.domain.services
{
    [Trait("Category", "Integration")]
    public class YelpServiceTests
    {
        private string _apiKey = System.Environment.GetEnvironmentVariable("YELPAPIKEY");
        
        [Fact]
        public async void YelpService_GetAvailableRestaurantOptionsAsync_ReturnsTotalNumberOfAvailableResults()
        {
            // arrange
            int total = 0;
            YelpService target = new YelpService(_apiKey);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);
                HttpResponseMessage message = await client.GetAsync("https://api.yelp.com/v3/businesses/search?categories=restaurants,!hotdogs&location=38655&radius=20000&limit=50&sort_by=best_match");
                dynamic dto = JsonConvert.DeserializeObject<dynamic>(await message.Content.ReadAsStringAsync().ConfigureAwait(false));
                total = dto.total;
            }

            // act
            IEnumerable<RestaurantDto> result = await target.GetAvailableRestaurantOptionsAsync();

            // assert
            Assert.Equal(total, result.Count());

        }

    }
}
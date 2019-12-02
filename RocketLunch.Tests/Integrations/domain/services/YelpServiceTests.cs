using System.Collections.Generic;
using RocketLunch.domain.dtos;
using RocketLunch.domain.services;
using Xunit;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using RocketLunch.domain.contracts;
using Moq;
using RocketLunch.domain.services.mocks;
using RocketLunch.domain.enumerations;

namespace RocketLunch.tests.integrations.domain.services
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
            RestaurantCacheMock mockCache = new RestaurantCacheMock();
            YelpService target = new YelpService(_apiKey, mockCache);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);
                HttpResponseMessage message = await client.GetAsync("https://api.yelp.com/v3/businesses/search?categories=restaurants,!hotdogs&location=38655&radius=20000&limit=50&sort_by=best_match");
                dynamic dto = JsonConvert.DeserializeObject<dynamic>(await message.Content.ReadAsStringAsync().ConfigureAwait(false));
                total = dto.total;
            }

            // act
            DateTime startTime = DateTime.UtcNow;
            IEnumerable<RestaurantDto> result = await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions { Meal = MealTime.all });
            DateTime endTime = DateTime.UtcNow;
            TimeSpan firstWatch = endTime - startTime;
            startTime = DateTime.UtcNow;
            await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions { Meal = MealTime.all });
            endTime = DateTime.UtcNow;
            TimeSpan secondWatch = endTime - startTime;
            startTime = DateTime.UtcNow;
            await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions { Meal = MealTime.dinner });
            endTime = DateTime.UtcNow;
            TimeSpan thirdWatch = endTime - startTime;

            // assert
            Assert.Equal(total, result.Count());
            Assert.True(firstWatch.TotalMilliseconds > 1000);
            Assert.True(secondWatch.TotalMilliseconds < 200);
            Assert.True(thirdWatch.TotalMilliseconds > 1000);

        }

        [Fact]
        public async void YelpService_GetAvailableRestaurantOptionsAsync_ReturnsFewerResultsWhenSpecifyingMeal()
        {
            // arrange
            RestaurantCacheMock mockCache = new RestaurantCacheMock();
            YelpService target = new YelpService(_apiKey, new Mock<IRestaurantCache>().Object);

            // act
            IEnumerable<RestaurantDto> result = await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions { Meal = MealTime.all });
            IEnumerable<RestaurantDto> result2 = await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions { Meal = MealTime.breakfast });

            // assert
            Assert.True(result.Count() > result2.Count());
        }

        [Fact]
        public async void YelpService_GetAvailableRestaurantOptionsAsync_ReturnsFewerResultsWhenSpecifyingCategory()
        {
            // arrange
            RestaurantCacheMock mockCache = new RestaurantCacheMock();
            YelpService target = new YelpService(_apiKey, new Mock<IRestaurantCache>().Object);

            // act
            IEnumerable<RestaurantDto> result = await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions());
            IEnumerable<RestaurantDto> result2 = await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions { Category = Category.vegetarian });

            // assert
            Assert.True(result.Count() > result2.Count());
        }

        [Fact]
        public async void YelpService_GetAvailableRestaurantOptionsAsync_ReturnsDifferentRestaurantsWhenGivenDifferentLocations()
        {
            // arrange
            RestaurantCacheMock mockCache = new RestaurantCacheMock();
            YelpService target = new YelpService(_apiKey, new Mock<IRestaurantCache>().Object);

            // act
            IEnumerable<RestaurantDto> result = await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions { Location = "38655" });
            IEnumerable<RestaurantDto> result2 = await target.GetAvailableRestaurantOptionsAsync(Guid.Empty, new SearchOptions { Location = "38834" });

            // assert
            Assert.NotEqual(result, result2);
        }


    }
}
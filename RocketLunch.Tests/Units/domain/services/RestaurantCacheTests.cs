using System;
using System.Collections.Generic;
using Moq;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.enumerations;
using RocketLunch.tests.builders;
using Xunit;

namespace RocketLunch.tests.units.domain.services
{
    [Trait("Category", "Unit")]
    public class RestaurantCacheTests
    {

        [Fact]
        public async void RestaurantCache_GetSeenOptions_ReturnsEmptyListWhenNotCached()
        {
            // // arrange
            Guid sessionId = Guid.NewGuid();

            Mock<ICache> cache = new Mock<ICache>();
            cache.Setup(x => x.GetAsync<List<string>>($"{sessionId.ToString()}_seenoptions")).ReturnsAsync(default(List<string>));
            var target = new RestaurantCacheBuilder()
                .SetCache(cache.Object)
                .Build();

            // // act
            var result = await target.GetSeenOptionsAsync(sessionId);

            // // assert
            Assert.Equal(new List<string>(), result);
        }

        [Fact]
        public async void RestaurantCache_GetSeenOptions_ReturnsOptionsWhenCached()
        {
            // // arrange
            Guid sessionId = Guid.NewGuid();
            Mock<ICache> cache = new Mock<ICache>();
            List<string> options = new List<string> {
                "bob",
                "john",
                "tim",
            };
            cache.Setup(x => x.GetAsync<List<string>>($"{sessionId.ToString()}_seenoptions")).ReturnsAsync(options);

            var target = new RestaurantCacheBuilder()
                .SetCache(cache.Object)
                .Build();

            // // act
            var result = await target.GetSeenOptionsAsync(sessionId);

            // // assert
            Assert.Equal(options, result);
        }

        [Fact]
        public async void RestaurantCache_AddSeenOptionAsync_CallsCacheSetWithSIngleOption()
        {
            // // arrange
            Guid sessionId = Guid.NewGuid();
            string option = "The Restaurant at the End of the Universe";

            Mock<ICache> cache = new Mock<ICache>();

            var target = new RestaurantCacheBuilder()
                .SetCache(cache.Object)
                .Build();

            // // act
            await target.AddSeenOptionAsync(sessionId, option);

            // assert
            cache.Verify(c => c.SetAsync<List<string>>($"{sessionId.ToString()}_seenoptions", It.Is<List<string>>(x => x.Contains(option)), null), Times.Once);
        }

        [Fact]
        public async void RestaurantCache_AddSeenOptionAsync_CallsCacheSetWhenCacheIsNonEmpty()
        {
            // arrange
            Guid sessionId = Guid.NewGuid();
            string option = "The Restaurant at the End of the Universe";
            const string existingOption = "Restaurant over there";
            List<string> list = new List<string>
            {
                existingOption
            };

            Mock<ICache> cache = new Mock<ICache>();
            cache.Setup(x => x.GetAsync<List<string>>(It.IsAny<String>())).ReturnsAsync(list);

            var target = new RestaurantCacheBuilder()
                .SetCache(cache.Object)
                .Build();

            // act
            await target.AddSeenOptionAsync(sessionId, option);

            // assert
            cache.Verify(c => c.SetAsync<List<string>>($"{sessionId.ToString()}_seenoptions", It.Is<List<string>>(x => x.Contains(option) && x.Contains(existingOption) && x.Count == 2), null), Times.Once);
        }

        // [Fact]
        // public async void RestaurantCache_SetUserSessionAsync_CallsSetCacheWithoutPreviousUserSession()
        // {
        //     // arrange
        //     Guid sessionId = Guid.NewGuid();
        //     List<int> userIds = new List<int> { 8, 6, 7, 5, 3, 0, 9, };

        //     Mock<ICache> cache = new Mock<ICache>();

        //     var target = new RestaurantCacheBuilder()
        //         .SetCache(cache.Object)
        //         .Build();

        //     // act
        //     await target.SetUserSessionAsync(sessionId, userIds);

        //     // assert
        //     cache.Verify(c => c.SetAsync<List<int>>($"{sessionId.ToString()}_usersessions", userIds, null), Times.Once);
        // }

        // [Fact]
        // public async void RestaurantCache_GetUserSessionAsync_ReturnNullWhenNotCached()
        // {
        //     // arrange
        //     Guid sessionId = Guid.NewGuid();
        //     List<int> userIds = new List<int> { 8, 6, 7, 5, 3, 0, 9, };

        //     Mock<ICache> cache = new Mock<ICache>();

        //     var target = new RestaurantCacheBuilder()
        //         .SetCache(cache.Object)
        //         .Build();

        //     // act
        //     List<int> result = await target.GetUserSessionAsync(sessionId);

        //     // assert
        //     Assert.Null(result);
        // }
        
        // [Fact]
        // public async void RestaurantCache_GetUserSessionAsync_ReturnListWhenCached()
        // {
        //     // arrange
        //     Guid sessionId = Guid.NewGuid();
        //     List<int> userIds = new List<int> { 8, 6, 7, 5, 3, 0, 9, };

        //     Mock<ICache> cache = new Mock<ICache>();
        //     cache.Setup(x => x.GetAsync<List<int>>($"{sessionId.ToString()}_usersessions")).ReturnsAsync(userIds);

        //     var target = new RestaurantCacheBuilder()
        //         .SetCache(cache.Object)
        //         .Build();

        //     // act
        //     List<int> result = await target.GetUserSessionAsync(sessionId);

        //     // assert
        //     Assert.Equal(userIds, result);
        // }

        [Fact]
        public async void RestaurantCache_GetRestaurantListAsync_ReturnsNullIfNotPresent()
        {
            // arrange
            Guid sessionId = Guid.NewGuid();

            var target = new RestaurantCacheBuilder()
                .Build();
            
            // act
            var result = await target.GetRestaurantListAsync(sessionId);
            
            // assert
            Assert.Null(result);
        }

        [Fact]
        public async void RestaurantCache_GetRestaurantListAsync_ReturnsListIfPresent()
        {
            // arrange
            Guid sessionId = Guid.NewGuid();
            Mock<ICache> cache = new Mock<ICache>();
            List<RestaurantDto> restaurants = new List<RestaurantDto> {
                new RestaurantDto {
                    Name = "bob's burgers",
                    Id = "bob"
                },
                new RestaurantDto {
                    Name = "Tim's tacos",
                    Id = "tim"
                }
            };
            cache.Setup(x => x.GetAsync<List<RestaurantDto>>($"{sessionId.ToString()}_sessionsearch")).ReturnsAsync(restaurants);

            var target = new RestaurantCacheBuilder()
                .SetCache(cache.Object)
                .Build();
            
            // act
            var result = await target.GetRestaurantListAsync(sessionId);
            
            // assert
            Assert.Equal(restaurants, result);
        }

        [Fact]
        public async void RestaurantCache_SetRestaurantListAsync_CallsSetCache()
        {
            // arrange
            Guid sessionId = Guid.NewGuid();
            Mock<ICache> cache = new Mock<ICache>();
            List<RestaurantDto> restaurants = new List<RestaurantDto> {
                new RestaurantDto {
                    Name = "bob's burgers",
                    Id = "bob"
                },
                new RestaurantDto {
                    Name = "Tim's tacos",
                    Id = "tim"
                }
            };

            var target = new RestaurantCacheBuilder()
                .SetCache(cache.Object)
                .Build();

            // act
            await target.SetRestaurantListAsync(sessionId, restaurants);

            // assert
            cache.Verify(c => c.SetAsync<List<RestaurantDto>>($"{sessionId.ToString()}_sessionsearch", restaurants, null), Times.Once);
        }

        [Fact]
        public async void RestaurantCache_SetSessionSearchOptionsAsync_CallsSetCache()
        {
            // arrange
            Guid sessionId = Guid.NewGuid();
            Mock<ICache> cache = new Mock<ICache>();
            var options = new SearchOptions {
                Meal = MealTime.breakfast
            };

            var target = new RestaurantCacheBuilder()
                .SetCache(cache.Object)
                .Build();

            // act
            await target.SetSessionSearchOptionsAsync(sessionId, options);

            // assert
            cache.Verify(c => c.SetAsync<SearchOptions>($"{sessionId.ToString()}_sessionsearchoptions", options, null), Times.Once);
        }

        [Fact]
        public async void RestaurantCache_GetSessionSearchOptionsAsync_CallsGetAsync()
        {
            // arrange
            Guid sessionId = Guid.NewGuid();
            Mock<ICache> cache = new Mock<ICache>();

            var options = new SearchOptions {
                Meal = MealTime.breakfast
            };

            cache.Setup(x => x.GetAsync<SearchOptions>($"{sessionId.ToString()}_sessionsearchoptions")).ReturnsAsync(options);

           

            var target = new RestaurantCacheBuilder()
                .SetCache(cache.Object)
                .Build();

            // act
            SearchOptions result = await target.GetSessionSearchOptionsAsync(sessionId);

            // assert
            Assert.Equal(options, result);
        }
    }
}
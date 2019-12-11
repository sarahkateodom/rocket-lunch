using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.services;
using Moq;
using Xunit;
using RocketLunch.domain.utilities;
using RocketLunch.domain.services.mocks;
using RocketLunch.domain.exceptions;

namespace RocketLunch.tests.units.domain.services
{
    [Trait("Category", "Unit")]
    public class LunchServiceTests
    {
        [Fact]
        public void LunchService_Ctor_RequiresIGetLunchOptions()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new LunchService((IGetLunchOptions)null, null, null, null));
        }

        [Fact]
        public void LunchService_Ctor_RequiresIRepository()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new LunchService(new Mock<IGetLunchOptions>().Object, (IRepository)null, null, null));
        }

        [Fact]
        public void LunchService_Ctor_RequiresIChaos()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new LunchService(new Mock<IGetLunchOptions>().Object, new Mock<IRepository>().Object, (IChaos)null, null));
        }

        [Fact]
        public void LunchService_Ctor_RequiresIRestaurantCache()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new LunchService(new Mock<IGetLunchOptions>().Object, new Mock<IRepository>().Object, new Mock<IChaos>().Object, (IRestaurantCache)null));
        }

        [Fact]
        public async void LunchService_GetRestaurantAsync_ReturnsRestaurantDto()
        {
            // arrange
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            Mock<IChaos> mockRandom = new Mock<IChaos>();
            Mock<IRestaurantCache> mockCache = new Mock<IRestaurantCache>();
            LunchService target = new LunchService(mockOptions.Object, mockRepo.Object, mockRandom.Object, mockCache.Object);
            const string expected = "bob's burgers";
            Guid sessionId = Guid.NewGuid();
            mockOptions.Setup(x => x.GetAvailableRestaurantOptionsAsync(sessionId, It.IsAny<SearchOptions>())).ReturnsAsync(new List<RestaurantDto> {
                new RestaurantDto {
                    Name = expected,
                },
            });

            // act
            var result = await target.GetRestaurantAsync(sessionId, new SearchOptions());

            // assert
            Assert.Equal(expected, result.Name);
        }

        [Fact]
        public async void LunchService_GetRestaurantAsync_ThrowTooManyRequestsWhenOutOfSuggestions()
        {
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            Mock<IChaos> mockRandom = new Mock<IChaos>();
            IRestaurantCache cache = new RestaurantCacheMock();
            const string expected = "bob's burgers";
            Guid sessionId = Guid.NewGuid();
            mockOptions.Setup(x => x.GetAvailableRestaurantOptionsAsync(sessionId, It.IsAny<SearchOptions>())).ReturnsAsync(new List<RestaurantDto> {
                new RestaurantDto {
                    Name = expected,
                },
            });
            LunchService target = new LunchService(mockOptions.Object, mockRepo.Object, mockRandom.Object, cache);

            // act
            var result = await target.GetRestaurantAsync(sessionId, new SearchOptions());

            // assert
            await Assert.ThrowsAsync<TooManyRequestsException>(async() => await target.GetRestaurantAsync(sessionId, new SearchOptions()));
        }

        [Fact]
        public async void LunchService_GetRestaurantAsync_ReturnsRestaurantDtosWithoutNopes()
        {
            // arrange
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            Mock<IChaos> mockRandom = new Mock<IChaos>();
            IRestaurantCache cache = new RestaurantCacheMock();
            mockRandom.Setup(m => m.Next(It.IsAny<int>())).Returns(0);
            Guid sessionGuid = Guid.NewGuid();
            mockOptions.Setup(x => x.GetAvailableRestaurantOptionsAsync(sessionGuid, It.IsAny<SearchOptions>())).ReturnsAsync(new List<RestaurantDto> {
                new RestaurantDto {
                    Name = "COok OOout",
                    Id = "rest3"
                },
                new RestaurantDto {
                    Name = "Jimmy Pesto's Pizzaria",
                    Id = "rest2"
                },
                new RestaurantDto {
                    Name = "Bob's Burgers",
                    Id = "rest1"
                },
            });
            mockRepo.Setup(x => x.GetUsersAsync()).ReturnsAsync(new List<UserDto>
            {
                new UserDto {
                    Id = 1,
                    Nopes = new List<string>{"rest2"}
                },
                new UserDto {
                    Id = 2,
                    Nopes = new List<string>{"rest3"}
                }
            });
            List<int> users = new List<int> { 1, 2 };
            LunchService target = new LunchService(mockOptions.Object, mockRepo.Object, mockRandom.Object, cache);

            // act
            var result = await target.GetRestaurantAsync(sessionGuid, new SearchOptions() { UserIds = users });

            // assert
            Assert.Equal("Bob's Burgers", result.Name);
        }

        [Fact]
        public async void LunchService_GetRestaurantsAsync_ReturnsRestaurantDtos()
        {
            // arrange
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            Mock<IChaos> mockRandom = new Mock<IChaos>();
            Mock<IRestaurantCache> mockCache = new Mock<IRestaurantCache>();
            LunchService target = new LunchService(mockOptions.Object, mockRepo.Object, mockRandom.Object, mockCache.Object);
            mockOptions.Setup(x => x.GetAllRestaurantsInZipAsync(It.IsAny<string>())).ReturnsAsync(new List<RestaurantDto> {
                new RestaurantDto {
                    Name = "Bob's Burgers",
                },
                new RestaurantDto {
                    Name = "Jimmy Pesto's Pizzaria",
                },
            });
            string zip = "38655";

            // act
            var result = await target.GetRestaurantsAsync(zip);

            // assert
            Assert.Equal(2, result.Count());
        }
    }
}
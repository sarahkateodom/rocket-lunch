using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using makelunch.domain.services;
using Moq;
using Xunit;
using makeLunch.domain.utilities;

namespace makelunch.tests.units.domain.services
{
    [Trait("Category", "Unit")]
    public class LunchServiceTests
    {
        [Fact]
        public void LunchService_Ctor_RequiresIGetLunchOptions()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new LunchService((IGetLunchOptions)null, null, null));
        }

        [Fact]
        public void LunchService_Ctor_RequiresIRepository()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new LunchService(new Mock<IGetLunchOptions>().Object, (IRepository)null, null));
        }

        [Fact]
        public void LunchService_Ctor_RequiresIChaos()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new LunchService(new Mock<IGetLunchOptions>().Object, new Mock<IRepository>().Object, (IChaos)null));
        }

        [Fact]
        public async void LunchService_GetRestaurantAsync_ReturnsRestaurantDto()
        {
            // arrange
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            Mock<IChaos> mockRandom = new Mock<IChaos>();
            LunchService target = new LunchService(mockOptions.Object, mockRepo.Object, mockRandom.Object);
            const string expected = "bob's burgers";
            mockOptions.Setup(x => x.GetAvailableRestaurantOptionsAsync()).ReturnsAsync(new List<RestaurantDto> {
                new RestaurantDto {
                    Name = expected,
                },
            });

            // act
            var result = await target.GetRestaurantAsync(Guid.NewGuid());

            // assert
            result.Match(
                err => throw new Exception("Unexpected exception."),
                x => Assert.Equal(expected, x.Name)
            );
        }

        [Fact]
        public async void LunchService_GetRestaurantAsync_ThrowTooManyRequestsWhenOutOfSuggestions()
        {
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            Mock<IChaos> mockRandom = new Mock<IChaos>();
            const string expected = "bob's burgers";
            mockOptions.Setup(x => x.GetAvailableRestaurantOptionsAsync()).ReturnsAsync(new List<RestaurantDto> {
                new RestaurantDto {
                    Name = expected,
                },
            });
            LunchService target = new LunchService(mockOptions.Object, mockRepo.Object, mockRandom.Object);
            Guid sessionId = Guid.NewGuid();

            // act
            var result = await target.GetRestaurantAsync(sessionId);
            result = await target.GetRestaurantAsync(sessionId);

            // assert
            result.Match(
                err => Assert.Equal(HttpStatusCode.TooManyRequests, err.HttpErrorStatusCode),
                x => throw new Exception("no exception thrown")
            );
        }

        [Fact]
        public async void LunchService_GetRestaurantAsync_ReturnsRestaurantDtosWithoutNopes()
        {
            // arrange
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            Mock<IChaos> mockRandom = new Mock<IChaos>();
            mockRandom.Setup(m => m.Next(It.IsAny<int>())).Returns(0);
            mockOptions.Setup(x => x.GetAvailableRestaurantOptionsAsync()).ReturnsAsync(new List<RestaurantDto> {
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
            Guid sessionGuid = Guid.NewGuid();
            List<int> users = new List<int> { 1, 2 };
            LunchService target = new LunchService(mockOptions.Object, mockRepo.Object, mockRandom.Object);
            RestaurantCash.CreateUpdateUserSession(sessionGuid, users);

            // act
            var result = await target.GetRestaurantAsync(sessionGuid);

            // assert
            result.Match(
                err => throw new Exception("Unexpected exception."),
                x => Assert.Equal("Bob's Burgers", x.Name)
            );
        }

        public async void LunchService_GetRestaurantsAsync_ReturnsRestaurantDtos()
        {
            // arrange
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            Mock<IChaos> mockRandom = new Mock<IChaos>();
            LunchService target = new LunchService(mockOptions.Object, mockRepo.Object, mockRandom.Object);
            mockOptions.Setup(x => x.GetAvailableRestaurantOptionsAsync()).ReturnsAsync(new List<RestaurantDto> {
                new RestaurantDto {
                    Name = "Bob's Burgers",
                },
                new RestaurantDto {
                    Name = "Jimmy Pesto's Pizzaria",
                },
            });

            // act
            var result = await target.GetRestaurantsAsync();

            // assert
            result.Match(
                err => throw new Exception("Unexpected exception."),
                x => Assert.Equal(2, x.Count())
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using makelunch.domain.services;
using Moq;
using Xunit;

namespace makelunch.tests.units.domain.services
{
    [Trait("Category", "Unit")]
    public class LunchServiceTests
    {
        [Fact]
        public void LunchService_Ctor_RequiresIGetLunchOptions()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new LunchService((IGetLunchOptions)null));
        }

        [Fact]
        public async void LunchService_GetRestaurantAsync_ReturnsRestaurantDto()
        {
            // arrange
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            LunchService target = new LunchService(mockOptions.Object);
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
            const string expected = "bob's burgers";
            mockOptions.Setup(x => x.GetAvailableRestaurantOptionsAsync()).ReturnsAsync(new List<RestaurantDto> {
                new RestaurantDto {
                    Name = expected,
                },
            });
            LunchService target = new LunchService(mockOptions.Object);
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

        public async void LunchService_GetRestaurantsAsync_ReturnsRestaurantDtos()
        {
            // arrange
            Mock<IGetLunchOptions> mockOptions = new Mock<IGetLunchOptions>();
            LunchService target = new LunchService(mockOptions.Object);
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
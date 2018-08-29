using System;
using System.Collections.Generic;
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
            var result = await target.GetRestaurantAsync();

            // assert
            result.Match(
                err => throw new Exception("Unexpected exception."),
                x => Assert.Equal(expected, x.Name)
            );
        }

    }
}
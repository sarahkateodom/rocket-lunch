using System;
using RocketLunch.domain.utilities;
using Xunit;

namespace RocketLunch.tests.units.domain.utilities
{
    [Trait("Category", "Unit")]
    public class RestaurantCashTests 
    {
        [Fact]
        public void RestaurantCash_GetSeenOptions_ShouldReturnNullWhenEmpty()
        {
            // act
            var result = RestaurantCash.GetSeenOptions(Guid.NewGuid().ToString());
        
            // assert
            Assert.Null(result);
        }

        [Fact]
        public void RestaurantCash_AddSeenOptions_AddsOption()
        {
            // arrange
            string sessionId = Guid.NewGuid().ToString();

            // act
            RestaurantCash.AddSeenOption(sessionId, "bob's burgers");

            // assert
            var result = RestaurantCash.GetSeenOptions(sessionId);
            Assert.Contains("bob's burgers", result);
        }
        
    }
}
using System;
using makeLunch.domain.utilities;
using Xunit;

namespace makeLunch.tests.units.domain.utilities
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
        public void RestaurantCash_AddSeenOptions_ShouldAllSeenOptions()
        {
            string sessionId = Guid.NewGuid().ToString();
            RestaurantCash.AddSeenOption(sessionId, "bob's burgers");
            var result = RestaurantCash.GetSeenOptions(sessionId);
            Assert.Contains("bob's burgers", result);
            RestaurantCash.AddSeenOption(sessionId, "snot's burgers");
            result = RestaurantCash.GetSeenOptions(sessionId);
            Assert.Contains("bob's burgers", result);
            Assert.Contains("snot's burgers", result);
        }
        
    }
}
using Moq;
using RocketLunch.domain.contracts;
using RocketLunch.domain.services;

namespace RocketLunch.tests.builders
{
    public class RestaurantCacheBuilder
    {
        private ICache cache;
        public RestaurantCacheBuilder()
        {
            this.cache = new Mock<ICache>().Object;
        }

        public RestaurantCache Build()
        {
            return new RestaurantCache(this.cache);
        }

        public RestaurantCacheBuilder SetCache(ICache cache)
        {
            this.cache = cache;
            return this;
        }
    }
}
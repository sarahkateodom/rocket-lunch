using System;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using Microsoft.AspNetCore.Mvc;
using RocketLunch.domain.dtos;
using RocketLunch.domain.enumerations;

namespace RocketLunch.web.controllers
{
    public class RestaurantsController : Controller
    {
        private IServeLunch _serveLunch;

        public RestaurantsController(IServeLunch serveLunch)
        {
            _serveLunch = serveLunch ?? throw new ArgumentNullException("serveLunch");
        }

        [HttpGet]
        [Route("api/restaurants/{sessionId}/{meal=0}")]
        public async Task<ObjectResult> GetRestaurant(Guid sessionId, MealTime meal)
        {
            return await this.GetRestaurant(sessionId, new SearchOptions() { Meal = meal });
        }

        private async Task<ObjectResult> GetRestaurant(Guid sessionId, SearchOptions options)
        {
            var result = await _serveLunch.GetRestaurantAsync(sessionId, options);
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }

        [HttpGet]
        [Route("api/restaurants")]
        public async Task<ObjectResult> GetRestaurants()
        {
            var result = await _serveLunch.GetRestaurantsAsync();
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }
    }
}
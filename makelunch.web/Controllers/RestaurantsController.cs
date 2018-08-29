using System;
using System.Threading.Tasks;
using makelunch.domain.contracts;
using Microsoft.AspNetCore.Mvc;

namespace makelunch.web.controllers
{
    public class RestaurantsController : Controller
    {
        private IServeLunch _serveLunch;

        public RestaurantsController(IServeLunch serveLunch)
        {
            _serveLunch = serveLunch ?? throw new ArgumentNullException("serveLunch");
        }

        [HttpGet]
        [Route("api/restaurants")]
        public async Task<ObjectResult> GetRestaurant()
        {
            var result = await _serveLunch.GetRestaurantAsync();
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }
    }
}
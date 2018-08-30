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
        [Route("api/restaurants/{sessionId}")]
        public async Task<ObjectResult> GetRestaurant(Guid sessionId)
        {
            var result = await _serveLunch.GetRestaurantAsync(sessionId);
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }
    }
}
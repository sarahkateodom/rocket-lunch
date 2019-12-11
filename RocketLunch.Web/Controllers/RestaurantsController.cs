﻿using System;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using Microsoft.AspNetCore.Mvc;
using RocketLunch.domain.dtos;
using RocketLunch.domain.enumerations;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Collections.Generic;

namespace RocketLunch.web.controllers
{
    public class RestaurantsController : Controller
    {
        private IServeLunch _serveLunch;

        public RestaurantsController(IServeLunch serveLunch)
        {
            _serveLunch = serveLunch ?? throw new ArgumentNullException("serveLunch");
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, "Search restaurants by SearchOptions", typeof(RestaurantDto))]
        [Route("api/restaurants/{sessionId}")]
        public async Task<ObjectResult> GetRestaurant(Guid sessionId, SearchOptions options)
        {
            var result = await _serveLunch.GetRestaurantAsync(sessionId, options);
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all restaurants for a ZIP", typeof(List<RestaurantDto>))]
        [Route("api/restaurants/{zip}")]
        public async Task<ObjectResult> GetRestaurantsForZip(string zip)
        {
            // This collection is used to populate the Nope list in user profile
            return new OkObjectResult(new List<RestaurantDto>());
        }
    }
}
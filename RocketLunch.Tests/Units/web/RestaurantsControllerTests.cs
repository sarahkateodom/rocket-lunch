using System;
using System.Collections.Generic;
using System.Net;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.web.controllers;
using RocketLunch.domain.utilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using RocketLunch.domain.enumerations;
using RocketLunch.domain.exceptions;

namespace RocketLunch.tests.web
{
    [Trait("Category", "Unit")]
    public class RestaurantsControllerTests
    {
        [Fact]
        public void RestaurantsController_Ctor_RequiresLunchService()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RestaurantsController((IServeLunch)null));
        }

        // [Fact]
        // public async void RestaurantsController_GetRestaurant_CallsServeLunchGetRestaurantAsync()
        // {
        //     // Arrange
        //     Mock<IServeLunch> mockLunch = new Mock<IServeLunch>();
        //     mockLunch.Setup(s => s.GetRestaurantAsync(It.IsAny<Guid>(), It.IsAny<SearchOptions>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, RestaurantDto>().Create(new RestaurantDto()));
        //     var target = new RestaurantsController(mockLunch.Object);

        //     // Act
        //     var result = await target.GetRestaurant(Guid.NewGuid(), MealTime.all);

        //     //Assert
        //     Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        //     mockLunch.Verify(v => v.GetRestaurantAsync(It.IsAny<Guid>(), It.IsAny<SearchOptions>()), Times.Once);
        // }

        // [Fact]
        // public async void RestaurantsController_GetRestaurant_ReturnsErrorStatusOnException()
        // {
        //     // Arrange
        //     Mock<IServeLunch> mockLunch = new Mock<IServeLunch>();
        //     mockLunch.Setup(s => s.GetRestaurantAsync(It.IsAny<Guid>(), It.IsAny<SearchOptions>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, RestaurantDto>().Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "testing")));
        //     var target = new RestaurantsController(mockLunch.Object);

        //     // Act
        //     var result = await target.GetRestaurant(Guid.NewGuid(), MealTime.all);

        //     //Assert
        //     Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        //     mockLunch.Verify(v => v.GetRestaurantAsync(It.IsAny<Guid>(), It.IsAny<SearchOptions>()), Times.Once);
        // }

        [Fact]
        public async void RestaurantsController_GetRestaurants_CallsServeLunchGetRestaurantsAsync()
        {
            // Arrange
            Mock<IServeLunch> mockLunch = new Mock<IServeLunch>();
            mockLunch.Setup(s => s.GetRestaurantsAsync(It.IsAny<string>())).ReturnsAsync(new List<RestaurantDto>());
            var target = new RestaurantsController(mockLunch.Object);
            var zip = "38655";

            // Act
            var result = await target.GetRestaurantsForZip(zip);

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            mockLunch.Verify(v => v.GetRestaurantsAsync(zip), Times.Once);
        }
    }
}
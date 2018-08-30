using System;
using System.Collections.Generic;
using System.Net;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using makelunch.web.controllers;
using makeLunch.domain.utilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace makelunch.tests.web
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

        [Fact]
        public async void RestaurantsController_GetRestaurant_CallsServeLunchGetRestaurantAsync()
        {
            // Arrange
            Mock<IServeLunch> mockLunch = new Mock<IServeLunch>();
            mockLunch.Setup(s => s.GetRestaurantAsync(It.IsAny<Guid>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, RestaurantDto>().Create(new RestaurantDto()));
            var target = new RestaurantsController(mockLunch.Object);

            // Act
            var result = await target.GetRestaurant(Guid.NewGuid());

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            mockLunch.Verify(v => v.GetRestaurantAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void RestaurantsController_GetRestaurant_ReturnsErrorStatusOnException()
        {
            // Arrange
            Mock<IServeLunch> mockLunch = new Mock<IServeLunch>();
            mockLunch.Setup(s => s.GetRestaurantAsync(It.IsAny<Guid>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, RestaurantDto>().Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "testing")));
            var target = new RestaurantsController(mockLunch.Object);

            // Act
            var result = await target.GetRestaurant(Guid.NewGuid());

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            mockLunch.Verify(v => v.GetRestaurantAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void RestaurantsController_GetRestaurants_ReturnsErrorStatusOnException()
        {
            // Arrange
            Mock<IServeLunch> mockLunch = new Mock<IServeLunch>();
            mockLunch.Setup(s => s.GetRestaurantsAsync()).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, IEnumerable<RestaurantDto>>().Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "testing")));
            var target = new RestaurantsController(mockLunch.Object);
            
            // Act
            var result = await target.GetRestaurants();
            
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            mockLunch.Verify(v => v.GetRestaurantsAsync(), Times.Once);
        }

        [Fact]
        public async void RestaurantsController_GetRestaurants_CallsServeLunchGetRestaurantsAsync()
        {
            // Arrange
            Mock<IServeLunch> mockLunch = new Mock<IServeLunch>();
            mockLunch.Setup(s => s.GetRestaurantsAsync()).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, IEnumerable<RestaurantDto>>().Create(new List<RestaurantDto>()));
            var target = new RestaurantsController(mockLunch.Object);
            
            // Act
            var result = await target.GetRestaurants();
            
            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            mockLunch.Verify(v => v.GetRestaurantsAsync(), Times.Once);
        }
    }
}
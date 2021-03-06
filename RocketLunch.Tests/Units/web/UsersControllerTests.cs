using System;
using System.Collections.Generic;
using System.Net;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.web.controllers;
using RocketLunch.domain.utilities;
using Moq;
using Xunit;
using RocketLunch.domain.exceptions;

namespace RocketLunch.tests.web
{
    [Trait("Category", "Unit")]
    public class UsersControllerTests
    {
        [Fact]
        public void UsersControllerTests_Ctor_RequiresuserService()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UsersController((IManageUsers)null, (IManageClaims)null));
        }

        // [Fact]
        // public async void UsersControllerTests_GetUsers_ReturnsHttpErrorOnException()
        // {
        //     // Arrange
        //     Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
        //     mockUserService.Setup(s => s.GetUsersAsync()).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, IEnumerable<UserDto>>().Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "testing")));
        //     var target = new UsersController(mockUserService.Object);
        //     CreateUserDto dto = new CreateUserDto();

        //     // Act
        //     var result = await target.GetUsers();

        //     //Assert
        //     Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        //     mockUserService.Verify(v => v.GetUsersAsync(), Times.Once);
        // }

        // [Fact]
        // public async void UsersControllerTests_GetUsers_ReturnsUsersOnSuccess()
        // {
        //     // Arrange
        //     Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
        //     mockUserService.Setup(s => s.GetUsersAsync()).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, IEnumerable<UserDto>>().Create(new List<UserDto>()));
        //     var target = new UsersController(mockUserService.Object);
        //     CreateUserDto dto = new CreateUserDto();

        //     // Act
        //     var result = await target.GetUsers();

        //     //Assert
        //     Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        //     mockUserService.Verify(v => v.GetUsersAsync(), Times.Once);
        // }

        [Fact]
        public async void UsersControllerTests_UpdateUser_CallsUserServiceUpdateUserAsync()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            Mock<IManageClaims> mockClaimsService = new Mock<IManageClaims>();
            mockUserService.Setup(s => s.UpdateUserAsync(It.IsAny<int>(), It.IsAny<UserUpdateDto>())).ReturnsAsync(true);
            var target = new UsersController(mockUserService.Object, mockClaimsService.Object);
            UserUpdateDto dto = new UserUpdateDto();
            int userId = 1;

            // Act
            var result = await target.UpdateUser(1, dto);

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(true, result.Value);
            mockUserService.Verify(v => v.UpdateUserAsync(userId, dto), Times.Once);
        }
    }
}
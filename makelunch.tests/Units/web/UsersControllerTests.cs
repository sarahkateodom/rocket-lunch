using System;
using System.Collections.Generic;
using System.Net;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using makelunch.web.controllers;
using makeLunch.domain.utilities;
using Moq;
using Xunit;

namespace makelunch.tests.web
{
    [Trait("Category", "Unit")]
    public class UsersControllerTests
    {
         [Fact]
        public void UsersControllerTests_Ctor_RequiresuserService()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UsersController((IManageUsers)null));
        }

        [Fact]
        public async void  UsersControllerTests_CreateUser_CallsUserServiceCreateUserAsync()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            const int id = 1;
            mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, int>().Create(id));
            var target = new UsersController(mockUserService.Object);
            CreateUserDto dto = new CreateUserDto();

            // Act
            var result = await target.CreateUser(dto);
            
            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(id, result.Value);
            mockUserService.Verify(v => v.CreateUserAsync(dto), Times.Once);
        }

        [Fact]
        public async void  UsersControllerTests_CreateUser_ReturnsHttpErrorOnException()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, int>().Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "testing")));
            var target = new UsersController(mockUserService.Object);
            CreateUserDto dto = new CreateUserDto();

            // Act
            var result = await target.CreateUser(dto);
            
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            mockUserService.Verify(v => v.CreateUserAsync(dto), Times.Once);
        }

        [Fact]
        public async void  UsersControllerTests_GetUsers_ReturnsHttpErrorOnException()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            mockUserService.Setup(s => s.GetUsersAsync()).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, IEnumerable<UserDto>>().Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "testing")));
            var target = new UsersController(mockUserService.Object);
            CreateUserDto dto = new CreateUserDto();

            // Act
            var result = await target.GetUsers();
            
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            mockUserService.Verify(v => v.GetUsersAsync(), Times.Once);
        }

        [Fact]
        public async void  UsersControllerTests_GetUsers_ReturnsUsersOnSuccess()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            mockUserService.Setup(s => s.GetUsersAsync()).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, IEnumerable<UserDto>>().Create(new List<UserDto>()));
            var target = new UsersController(mockUserService.Object);
            CreateUserDto dto = new CreateUserDto();

            // Act
            var result = await target.GetUsers();
            
            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            mockUserService.Verify(v => v.GetUsersAsync(), Times.Once);
        }

        [Fact]
        public async void  UsersControllerTests_UpdateUser_CallsUserServiceUpdateUserAsync()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            mockUserService.Setup(s => s.UpdateUserAsync(It.IsAny<UserDto>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, bool>().Create(true));
            var target = new UsersController(mockUserService.Object);
            UserDto dto = new UserDto();

            // Act
            var result = await target.UpdateUser(dto);
            
            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(true, result.Value);
            mockUserService.Verify(v => v.UpdateUserAsync(dto), Times.Once);
        }

        [Fact]
        public async void  UsersControllerTests_UpdateUser_ReturnsHttpErrorOnException()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            mockUserService.Setup(s => s.UpdateUserAsync(It.IsAny<UserDto>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, bool>().Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "testing")));
            var target = new UsersController(mockUserService.Object);
             UserDto dto = new UserDto();

            // Act
             var result = await target.UpdateUser(dto);
            
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            mockUserService.Verify(v => v.UpdateUserAsync(dto), Times.Once);
        }
    }
}
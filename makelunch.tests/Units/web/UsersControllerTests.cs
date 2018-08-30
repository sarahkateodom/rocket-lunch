using System;
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
        public async void  UsersControllerTestsrCreateUser_CallsUserServiceCreateUserAsync()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, bool>().Create(true));
            var target = new UsersController(mockUserService.Object);
            CreateUserDto dto = new CreateUserDto();

            // Act
            var result = await target.CreateUser(dto);
            
            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            mockUserService.Verify(v => v.CreateUserAsync(dto), Times.Once);
        }

        [Fact]
        public async void  UsersControllerTestsrCreateUser_ReturnsHttpErrorOnException()
        {
            // Arrange
            Mock<IManageUsers> mockUserService = new Mock<IManageUsers>();
            mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(new EitherFactory<HttpStatusCodeErrorResponse, bool>().Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, "testing")));
            var target = new UsersController(mockUserService.Object);
            CreateUserDto dto = new CreateUserDto();

            // Act
            var result = await target.CreateUser(dto);
            
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            mockUserService.Verify(v => v.CreateUserAsync(dto), Times.Once);
        }
    }
}
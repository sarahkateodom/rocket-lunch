using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using makelunch.domain.services;
using Moq;
using Xunit;

namespace makelunch.tests.units.domain.services
{
    [Trait("Category", "Unit")]
    public class UserServiceTests
    {
        [Fact]
        public void UserService_Ctor_RequiresRepository()
        {
            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => new UserService((IRepository)null));
        }

        [Fact]
        public async void UserService_CreateUserAsync_CallsRepositoryWithCorrectValues()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            UserService target = new UserService(mockRepo.Object);
            CreateUserDto dto = new CreateUserDto
            {
                Name = "Dyl Pickal",
                AvatarUrl = "https://gph.is/18NWdNy",
            };

            // act
            await target.CreateUserAsync(dto);

            // assert
            mockRepo.Verify(r => r.CreateUserAsync(dto.Name, dto.AvatarUrl), Times.Once);
        }

        [Fact]
        public async void UserService_CreateUserAsync_ReturnsId()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            const int id = 1;
            mockRepo.Setup(r => r.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(id);
            UserService target = new UserService(mockRepo.Object);
            CreateUserDto dto = new CreateUserDto
            {

                Name = "Dyl Pickal",
                AvatarUrl = "https://gph.is/18NWdNy",
            };

            // act
            var result = await target.CreateUserAsync(dto);

            // assert
            result.Match(
                err => throw new Exception("Unexcpected exception was not thrown."),
                x => Assert.Equal(id, x)
            );
        }

        [Fact]
        public async void UserService_CreateUserAsync_ThrowsExceptionWithNullDto()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            UserService target = new UserService(mockRepo.Object);

            // act
            var result = await target.CreateUserAsync((CreateUserDto)null);

            // assert
            result.Match(
                err => Assert.Equal(HttpStatusCode.BadRequest, err.HttpErrorStatusCode),
                x => throw new Exception("ValidationException was not thrown.")
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void UserService_CreateUserAsync_ThrowsExceptionWithNullOrEmptyName(string name)
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            UserService target = new UserService(mockRepo.Object);
            CreateUserDto dto = new CreateUserDto
            {
                Name = name,
                AvatarUrl = "https://gph.is/18NWdNy",
            };

            // act
            var result = await target.CreateUserAsync(dto);

            // assert
            result.Match(
                err => Assert.Equal(HttpStatusCode.BadRequest, err.HttpErrorStatusCode),
                x => throw new Exception("ValidationException was not thrown.")
            );
        }

        [Fact]
        public async void UserService_GetUsersAsync_ReturnsListOfUsers()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetUsersAsync()).ReturnsAsync(new List<UserDto> {
                new UserDto(), new UserDto()
            });
            UserService target = new UserService(mockRepo.Object);

            // act
            var result = await target.GetUsersAsync();

            // assert
            result.Match(
                err => throw new Exception("Unexpected exception."),
                x => Assert.Equal(2, x.Count())
            );
            
        }
    }
}
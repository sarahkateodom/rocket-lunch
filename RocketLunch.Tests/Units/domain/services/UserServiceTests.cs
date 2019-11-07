using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.services;
using Moq;
using Newtonsoft.Json;
using Xunit;
using System.Security.Claims;

namespace RocketLunch.tests.units.domain.services
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
        public async void UserService_LoginAsync_CallsRepoGetUserAsync()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync((UserDto)null);
            mockRepo.Setup(x => x.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new UserDto
            {
                Id = 1,
                Name = "name",
            });
            UserService target = new UserService(mockRepo.Object);
            var dto = new LoginDto()
            {
                GoogleId = "google id",
                Name = "name",
            };

            // act
            await target.LoginAsync(dto);

            // assert
            mockRepo.Verify(r => r.GetUserAsync(dto.GoogleId), Times.Once);
        }

        [Fact]
        public async void UserService_LoginAsync_ReturnsClaimsPrincipleWithExistingUserInfo()
        {
            // arrange
            var loginDto = new LoginDto()
            {
                GoogleId = "google id",
                Name = "name",
            };

            var existingUser = new UserDto
            {
                Id = 1,
                Name = "Bill",
                Nopes = new List<string> { "salad", "tuna", "tuna-salad", },
            };

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

            UserService target = new UserService(mockRepo.Object);

            // act
            var result = await target.LoginAsync(loginDto);

            // assert
            Assert.Equal(existingUser.Id, result.Id);
            Assert.Equal(existingUser.Name, result.Name);
        }

        [Fact]
        public async void UserService_LoginAsync_DoesNotCreateUserIfFound()
        {
            // arrange
            var loginDto = new LoginDto()
            {
                GoogleId = "google id",
                Name = "name",
            };

            var existingUser = new UserDto
            {
                Id = 1,
                Name = "Bill",
                Nopes = new List<string> { "salad", "tuna", "tuna-salad", },
            };

            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

            UserService target = new UserService(mockRepo.Object);

            // act
            var result = await target.LoginAsync(loginDto);

            // assert
            mockRepo.Verify(r => r.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async void UserService_LoginAsync_CreatesUserIfNotFound()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync((UserDto)null);
            mockRepo.Setup(x => x.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new UserDto
            {
                Id = 1,
                Name = "name",
            });
            UserService target = new UserService(mockRepo.Object);
            var dto = new LoginDto()
            {
                GoogleId = "google id",
                Name = "name",
                Email = "email",
            };

            // act
            await target.LoginAsync(dto);

            // assert
            mockRepo.Verify(r => r.CreateUserAsync(dto.GoogleId, dto.Email, dto.Name), Times.Once);
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
                Nopes = new List<string> { "https://gph.is/18NWdNy" },
            };

            // act
            await target.CreateUserAsync(dto);

            // assert
            mockRepo.Verify(r => r.CreateUserAsync_Old(dto.Name, dto.Nopes), Times.Once);
        }

        [Fact]
        public async void UserService_CreateUserAsync_ReturnsId()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            const int id = 1;
            mockRepo.Setup(r => r.CreateUserAsync_Old(It.IsAny<string>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(id);
            UserService target = new UserService(mockRepo.Object);
            CreateUserDto dto = new CreateUserDto
            {

                Name = "Dyl Pickal",
                Nopes = new List<string> { "https://gph.is/18NWdNy" },
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
                Nopes = new List<string> { "https://gph.is/18NWdNy" },
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

        [Fact]
        public async void UserService_UpdateUserAsync_CallsRepositoryUpdateUser()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new UserDto());
            UserService target = new UserService(mockRepo.Object);
            UserDto dto = new UserDto
            {
                Id = 1,
                Name = "Anne Telohp",
                Nopes = new List<string> { "Chicken Salad Chick" },
            };

            // act
            var result = await target.UpdateUserAsync(dto);

            // assert
            mockRepo.Verify(r => r.UpdateUserAsync(dto.Id, dto.Name, dto.Nopes), Times.Once);
        }

        [Fact]
        public async void UserService_UpdateUserAsync_ThrowsNotFoundWhenUserDoesNotExist()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetUserAsync(It.IsAny<int>())).ReturnsAsync((UserDto)null);
            UserService target = new UserService(mockRepo.Object);
            UserDto dto = new UserDto
            {
                Id = 1,
                Name = "Anne Telohp",
                Nopes = new List<string> { "Chicken Salad Chick" },
            };

            // act
            var result = await target.UpdateUserAsync(dto);

            // assert
            result.Match(
                err => Assert.Equal(HttpStatusCode.NotFound, err.HttpErrorStatusCode),
                x => throw new Exception("NotFoundException not thrown.")
            );
        }

    }
}
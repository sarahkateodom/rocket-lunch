using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.exceptions;
using RocketLunch.domain.services;
using Xunit;

namespace RocketLunch.tests.units.domain.services
{
    [Trait("Category", "UnitTest")]
    public class TeamServiceTests
    {
        [Fact]
        public async void TeamService_CreateTeamAsync_ThrowsArgumentNullExceptionWithNullDto()
        {
            // arrange
            TeamDto dto = (TeamDto)null;

            var repo = new Mock<IRepository>();
            var target = new TeamService(repo.Object);

            // act
            // assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await target.CreateTeamAsync(1, dto));
        }

        [Fact]
        public async void TeamService_CreateTeamAsync_ReturnsIdFromRepoCall()
        {
            // arrange
            TeamDto dto = new TeamDto
            {
                Name = "The Mighty Ducks",
                Zip = "38655",
            };

            int newTeamId = 32;

            var repo = new Mock<IRepository>();
            repo.Setup(r => r.CreateTeamAsync(dto.Name, dto.Zip)).ReturnsAsync(newTeamId);
            repo.Setup(r => r.TeamNameExistsAsync(dto.Name)).ReturnsAsync(false);

            var target = new TeamService(repo.Object);

            // act
            var result = await target.CreateTeamAsync(1, dto);

            // assert
            Assert.Equal(newTeamId, result.Id);
        }


        [Fact]
        public async void TeamService_CreateTeamAsync_AddsUserToTeam()
        {
            // arrange
            int userId = 42;
            TeamDto dto = new TeamDto
            {
                Name = "The Mighty Ducks",
                Zip = "38655",
            };

            int newTeamId = 32;

            var repo = new Mock<IRepository>();
            repo.Setup(r => r.CreateTeamAsync(dto.Name, dto.Zip)).ReturnsAsync(newTeamId);
            repo.Setup(r => r.TeamNameExistsAsync(dto.Name)).ReturnsAsync(false);

            var target = new TeamService(repo.Object);

            // act
            var result = await target.CreateTeamAsync(userId, dto);

            // assert
            repo.Verify(r => r.AddUserToTeamAsync(userId, result.Id), Times.Once);
        }

        [Fact]
        public async void TeamService_CreateTeamAsync_ThrowsBadRequestExceptionWithExistingTeamName()
        {
            // arrange
            TeamDto dto = new TeamDto
            {
                Name = "The Mighty Ducks",
                Zip = "38655",
            };

            int newTeamId = 32;

            var repo = new Mock<IRepository>();
            repo.Setup(r => r.CreateTeamAsync(dto.Name, dto.Zip)).ReturnsAsync(newTeamId);
            repo.Setup(r => r.TeamNameExistsAsync(dto.Name)).ReturnsAsync(true);

            var target = new TeamService(repo.Object);

            // act
            // assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await target.CreateTeamAsync(1, dto));
        }

        [Fact]
        public async void TeamService_UpdateTeamAsync_CallsRepositoryUpdateTeam()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetTeamAsync(It.IsAny<int>())).ReturnsAsync(new TeamDto());
            TeamService target = new TeamService(mockRepo.Object);
            int teamId = 1;
            TeamUpdateDto dto = new TeamUpdateDto
            {
                Name = "Anne Telohps",
                Zip = "90210"
            };

            // act
            var result = await target.UpdateTeamAsync(teamId, dto);

            // assert
            mockRepo.Verify(r => r.UpdateTeamAsync(teamId, dto.Name, dto.Zip), Times.Once);
        }

        [Fact]
        public async void TeamService_UpdateTeamAsync_ThrowsNotFoundWhenTeamDoesNotExist()
        {
            // arrange
            Mock<IRepository> mockRepo = new Mock<IRepository>();
            mockRepo.Setup(r => r.GetTeamAsync(It.IsAny<int>())).ReturnsAsync((TeamDto)null);
            TeamService target = new TeamService(mockRepo.Object);
            int teamId = 1;
            TeamUpdateDto dto = new TeamUpdateDto
            {
                Name = "Anne Telohps",
                Zip = "90210"
            };

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await target.UpdateTeamAsync(teamId, dto));
        }
        
        [Fact]
        public async void TeamService_AddUserToTeamAsync_CallRepo()
        {
            // arrange
            int teamId = 32;
            string email = "dogs@cats.com";
            int userId = 42;

            var repo = new Mock<IRepository>();
            repo.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(new UserWithTeamsDto
            {
                Id = userId
            });

            var target = new TeamService(repo.Object);

            // act
            await target.AddUserToTeamAsync(teamId, email);

            // assert
            repo.Verify(x => x.AddUserToTeamAsync(userId, teamId), Times.Once);
        }

        [Fact]
        public async void TeamService_AddUserToTeamAsync_DoesNotCallRepoWhenUserDoesNotExist()
        {
            // arrange
            int teamId = 32;
            string email = "dogs@cats.com";
            int userId = 42;

            var repo = new Mock<IRepository>();
            repo.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync((UserWithTeamsDto)null);

            var target = new TeamService(repo.Object);

            // act
            await Assert.ThrowsAsync<NotFoundException>(async() => await target.AddUserToTeamAsync(teamId, email));

            // assert
            repo.Verify(x => x.AddUserToTeamAsync(userId, teamId), Times.Never);
        }

        [Fact]
        public async void TeamService_AddUserToTeamAsync_ThrowsNotFoundExceptionWhenUserDoesNotExist()
        {
            // arrange
            int teamId = 32;
            string email = "dogs@cats.com";

            var repo = new Mock<IRepository>();
            repo.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync((UserWithTeamsDto)null);

            var target = new TeamService(repo.Object);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(async() => await target.AddUserToTeamAsync(teamId, email));
        }

        [Fact]
        public async void TeamService_GetUsersOfTeam_ReturnsUsersFromRepo()
        {
            // arrange
            int teamId = 32;

            var repo = new Mock<IRepository>();
            List<UserDto> users = new List<UserDto> {
                new UserDto {
                    Name = "bob"
                },
                new UserDto {
                    Name = "lilTimmy"
                }
            };
            repo.Setup(r => r.GetUsersOfTeamAsync(teamId)).ReturnsAsync(users);

            var target = new TeamService(repo.Object);

            // act
            var result = await target.GetUsersOfTeamAsync(teamId);

            // assert
            Assert.Equal(users, result);
        }

        [Fact]
        public async void TeamService_GetUsersOfTeam_ThrowsNotFoundWhenTeamIsNotThere()
        {
            // arrange
            int teamId = 32;

            var repo = new Mock<IRepository>();
            repo.Setup(r => r.GetUsersOfTeamAsync(teamId)).ReturnsAsync((IEnumerable<UserDto>)null);

            var target = new TeamService(repo.Object);

            // act
            // assert
            Exception ex = await Assert.ThrowsAsync<NotFoundException>( async () => await target.GetUsersOfTeamAsync(teamId));
            Assert.Contains("Team not found", ex.Message);
        }


        [Fact]
        public async void TeamService_RemoveUserFromTeamAsync_CallRepo()
        {
            // arrange
            int teamId = 32;
            int userId = 89;

            var repo = new Mock<IRepository>();
            var target = new TeamService(repo.Object);

            // act
            await target.RemoveUserFromTeamAsync(teamId, userId);

            // assert
            repo.Verify(x => x.RemoveUserFromTeamAsync(userId, teamId), Times.Once);
        }
    }
}
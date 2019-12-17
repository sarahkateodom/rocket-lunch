using System;
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
    }
}
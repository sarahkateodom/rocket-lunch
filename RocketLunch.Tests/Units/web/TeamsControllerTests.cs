using Xunit;
using System;
using Moq;
using RocketLunch.domain.contracts;
using RocketLunch.web.controllers;
using RocketLunch.domain.dtos;
using System.Collections.Generic;

namespace RocketLunch.tests.web
{
    [Trait("Category", "UnitTest")]
    public class TeamsControllerTests
    {
        [Fact]
        public async void TeamsController_CreateTeam_ReturnDtoFromTeamServiceCall()
        {
            // arrange
            int userId = 2;
            var teamDto = new TeamDto
            {
                Name = "bob's team",
                Zip = "90210"
            };

            Mock<IManageTeams> teamsService = new Mock<IManageTeams>();
            TeamDto returnedDto = new TeamDto { Name = teamDto.Name, Zip = teamDto.Zip, Id = 32, };
            teamsService.Setup(t => t.CreateTeamAsync(userId, teamDto)).ReturnsAsync(returnedDto);

            var target = new TeamsController(teamsService.Object);

            // act
            var result = await target.CreateTeam(userId, teamDto);

            // assert
            Assert.Equal(returnedDto, result.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void TeamsController_UpdateTeam_ReturnsOkAndCallsTeamService()
        {
            // Arrange
            Mock<IManageTeams> mockTeamService = new Mock<IManageTeams>();
            mockTeamService.Setup(s => s.UpdateTeamAsync(It.IsAny<int>(), It.IsAny<TeamUpdateDto>())).ReturnsAsync(true);
            var target = new TeamsController(mockTeamService.Object);
            TeamUpdateDto dto = new TeamUpdateDto();
            int teamId = 1;

            // Act
            var result = await target.UpdateTeam(1, dto);

            //Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(true, result.Value);
            mockTeamService.Verify(v => v.UpdateTeamAsync(teamId, dto), Times.Once);
        }

        [Fact]
        public async void TeamsController_AddUserToTeam_ReturnOkAndCallsTeamService()
        {
            // arrange
            string email = "bob@bob.com";
            int teamId = 32;

            Mock<IManageTeams> teamsService = new Mock<IManageTeams>();

            var target = new TeamsController(teamsService.Object);

            // act
            var result = await target.AddUserToTeam(email, teamId);

            // assert
            teamsService.Verify(x => x.AddUserToTeamAsync(teamId, email), Times.Once);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void TeamsController_GetTeamUsers_ReturnUserFromTeamService()
        {
            // arrange
            int teamId = 32;
            var users = new List<UserDto> {
                new UserDto {
                    Name = "bob"
                },
                new UserDto {
                    Name = "lilTimmy"
                }
            };

            Mock<IManageTeams> teamsService = new Mock<IManageTeams>();
            teamsService.Setup(x => x.GetUsersOfTeamAsync(teamId)).ReturnsAsync(users);

            var target = new TeamsController(teamsService.Object);

            // act
            var result = await target.GetTeamUsers(teamId);

            // assert
            Assert.Equal(users, result.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Trait("Category", "UnitTest")]
        [Fact]
        public async void TeamsController_RemoveUserFromTeam_CallsServiceAndReturnsOk()
        {
            // arrange
            int teamId = 23;
            int userId = 123;

            Mock<IManageTeams> teamsService = new Mock<IManageTeams>();
            var target = new TeamsController(teamsService.Object);

            // act
            var result = await target.RemoveUserFromTeam(userId, teamId);

            // assert
            teamsService.Verify(t => t.RemoveUserFromTeamAsync(teamId, userId), Times.Once);
            Assert.Equal(200, result.StatusCode);
        }

    }
}
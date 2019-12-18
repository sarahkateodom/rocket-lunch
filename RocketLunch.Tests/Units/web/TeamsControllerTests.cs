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
            teamsService.Setup(x => x.GetUsersOfTeam(teamId)).ReturnsAsync(users);

            var target = new TeamsController(teamsService.Object);

            // act
            var result = await target.GetTeamUsers(teamId);

            // assert
            Assert.Equal(users, result.Value);
            Assert.Equal(200, result.StatusCode);
        }


    }
}
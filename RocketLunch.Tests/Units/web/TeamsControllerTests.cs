using Xunit;
using System;
using Moq;
using RocketLunch.domain.contracts;
using RocketLunch.web.controllers;
using RocketLunch.domain.dtos;

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
            var teamDto = new TeamDto {
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
        
        
    }
}
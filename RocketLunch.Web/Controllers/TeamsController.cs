using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace RocketLunch.web.controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        IManageTeams teamsService;

        public TeamsController(IManageTeams teamsService)
        {
            this.teamsService = teamsService;
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, "Create new Team. This will add User to Team", typeof(TeamDto))]
        [Route("api/users/{userId}/teams")]
        public async Task<ObjectResult> CreateTeam(int userId, [FromBody] CreateTeamDto teamDto)
        {
            return new OkObjectResult(await this.teamsService.CreateTeamAsync(userId, teamDto));
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, "Add User to Team", typeof(UserDto))]
        [Route("api/users/{email}/teams/{teamId}")]
        public async Task<ObjectResult> AddUserToTeam(string email, int teamId)
        {
            UserDto result = await this.teamsService.AddUserToTeamAsync(teamId, email).ConfigureAwait(false);
            return new OkObjectResult(result);
        }

        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK, "Remove User from Team", typeof(bool))]
        [Route("api/users/{userId}/teams/{teamId}")]
        public async Task<ObjectResult> RemoveUserFromTeam(int userId, int teamId)
        {
            await this.teamsService.RemoveUserFromTeamAsync(teamId, userId);
            return new OkObjectResult(true);
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get Users in Team", typeof(IEnumerable<UserDto>))]
        [Route("api/teams/{teamId}/users")]
        public async Task<ObjectResult> GetTeamUsers(int teamId)
        {
            return new OkObjectResult(await this.teamsService.GetUsersOfTeamAsync(teamId));
        }
    }
}
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RocketLunch.domain.dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace RocketLunch.web.controllers
{
    public class TeamsController : Controller
    {
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, "Create new Team. This will add User to Team", typeof(TeamDto))]
        [Route("api/users/{userId}/teams")]
        public async Task<ObjectResult> CreateTeam(int userId, [FromBody] TeamDto teamDto)
        {
            return new OkObjectResult(new TeamDto());
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, "Add User to Team", typeof(TeamDto))]
        [Route("api/users/{email}/teams/{teamId}")]
        public async Task<ObjectResult> AddUserToTeam(string email, int teamId)
        {
            return new OkObjectResult(new TeamDto());
        }

        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK, "Remove User from Team", typeof(bool))]
        [Route("api/users/{userId}/teams/{teamId}")]
        public async Task<ObjectResult> RemoveUserFromTeam(int userId, int teamId)
        {
            return new OkObjectResult(true);
        }
    }
}
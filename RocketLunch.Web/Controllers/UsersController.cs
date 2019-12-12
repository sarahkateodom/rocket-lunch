using System;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace RocketLunch.web.controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private IManageUsers _userService;
        private IManageClaims _claims;

        public UsersController(IManageUsers userService, IManageClaims claimsService): base(claimsService)
        {
            _userService = userService ?? throw new ArgumentNullException("userService");
        }

        [HttpGet]
        [Route("api/users/current")]
        public async Task<ObjectResult> GetCurrentUser()
        {
            var userFromClaims = GetIdentityFromClaims();
            var result = await _userService.GetUserAsync(userFromClaims.Id);
            return new OkObjectResult(result);
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get User by internal identifier", typeof(UserDto))]
        [Route("api/users/{id}")]
        public async Task<ObjectResult> GetUser(int id)
        {
            var result = await _userService.GetUserAsync(id);
            return new OkObjectResult(result);
        }

        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, "Update User", typeof(bool))]
        [Route("api/users/{id}")]
        public async Task<ObjectResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            return new OkObjectResult(result);
        }
    }
}
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
    public class UsersController : Controller
    {
        private IManageUsers _userService;

        public UsersController(IManageUsers userService)
        {
            _userService = userService ?? throw new ArgumentNullException("userService");
        }

        // [HttpPost]
        // [Route("api/users")]
        // public async Task<ObjectResult> CreateUser([FromBody] CreateUserDto dto)
        // {
        //     var result = await _userService.CreateUserAsync(dto);
        //     return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        // }

        // [HttpGet]
        // [Route("api/users")]
        // public async Task<ObjectResult> GetUsers()
        // {
        //     var result = await _userService.GetUsersAsync();
        //     return result.Match(err => err.Content(this), r => new OkObjectResult(r ));
        // }

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
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }
    }
}
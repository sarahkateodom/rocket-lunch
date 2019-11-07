using System;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        [Route("api/users")]
        public async Task<ObjectResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }

        [HttpGet]
        [Route("api/users")]
        public async Task<ObjectResult> GetUsers()
        {
            var result = await _userService.GetUsersAsync();
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }

        [HttpPut]
        [Route("api/users")]
        public async Task<ObjectResult> UpdateUser([FromBody] UserDto dto)
        {
            var result = await _userService.UpdateUserAsync(dto);
            return result.Match(err => err.Content(this), r => new OkObjectResult(r));
        }
    }
}
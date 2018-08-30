using System;
using System.Threading.Tasks;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using Microsoft.AspNetCore.Mvc;

namespace makelunch.web.controllers
{
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
    }
}
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;

namespace RocketLunch.web.controllers
{
    public class LoginController : Controller
    {
        private IManageUsers _userService;
        public LoginController(IManageUsers userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<ObjectResult> Login([Bind][FromBody]LoginDto userDto)
        {
            ClaimsPrincipal result = await _userService.LoginAsync(userDto);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                result);
            return new OkObjectResult(userDto);
        }

    
        [HttpGet]
        [Route("api/logout")]
        public async Task<bool> Logout()
        {
            await HttpContext.SignOutAsync().ConfigureAwait(false);
            return true;
        }
    }
}
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Logs user in")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successful login", typeof(UserDto))]
        [Route("api/login")]
        public async Task<ObjectResult> Login([Bind][FromBody]LoginDto loginDto)
        {
            UserDto userDto = await _userService.LoginAsync(loginDto);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, "IFixIt", "ADMIN");
            identity.AddClaim(new Claim(ClaimTypes.Sid, userDto.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, userDto.Name));
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return new OkObjectResult(userDto);
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Logs user out")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Successful logout", typeof(bool))]
        [Route("api/logout")]
        public async Task<bool> Logout()
        {
            await HttpContext.SignOutAsync().ConfigureAwait(false);
            return true;
        }
    }
}
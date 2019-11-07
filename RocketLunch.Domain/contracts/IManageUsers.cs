using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;
using RocketLunch.domain.utilities;

namespace RocketLunch.domain.contracts
{
    public interface IManageUsers
    {
        Task<ClaimsPrincipal> LoginAsync(LoginDto userDto);
        Task<Either<HttpStatusCodeErrorResponse, int>> CreateUserAsync(CreateUserDto dto);
        Task<Either<HttpStatusCodeErrorResponse, IEnumerable<UserDto>>> GetUsersAsync();
        Task<Either<HttpStatusCodeErrorResponse, bool>> UpdateUserAsync(UserDto dto);
    }
}
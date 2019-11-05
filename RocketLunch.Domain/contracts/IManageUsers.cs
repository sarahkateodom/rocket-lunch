using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;
using RocketLunch.domain.utilities;

namespace RocketLunch.domain.contracts
{
    public interface IManageUsers
    {
        Task<Either<HttpStatusCodeErrorResponse, int>> CreateUserAsync(CreateUserDto dto);
        Task<Either<HttpStatusCodeErrorResponse, IEnumerable<UserDto>>> GetUsersAsync();
        Task<Either<HttpStatusCodeErrorResponse, bool>> UpdateUserAsync(UserDto dto);
    }
}
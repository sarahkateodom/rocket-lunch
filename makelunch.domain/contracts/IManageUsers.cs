using System.Collections.Generic;
using System.Threading.Tasks;
using makelunch.domain.dtos;
using makeLunch.domain.utilities;

namespace makelunch.domain.contracts
{
    public interface IManageUsers
    {
        Task<Either<HttpStatusCodeErrorResponse, int>> CreateUserAsync(CreateUserDto dto);
        Task<Either<HttpStatusCodeErrorResponse, IEnumerable<UserDto>>> GetUsersAsync();
    }
}
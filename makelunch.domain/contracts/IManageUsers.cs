using System.Threading.Tasks;
using makelunch.domain.dtos;
using makeLunch.domain.utilities;

namespace makelunch.domain.contracts
{
    public interface IManageUsers
    {
        Task<Either<HttpStatusCodeErrorResponse, bool>> CreateUserAsync(CreateUserDto dto);
    }
}
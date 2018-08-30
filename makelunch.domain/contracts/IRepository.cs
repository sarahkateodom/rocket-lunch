using System.Collections.Generic;
using System.Threading.Tasks;
using makelunch.domain.dtos;

namespace makelunch.domain.contracts
{
    public interface IRepository
    {
        Task<int> CreateUserAsync(string name, string avatarUrl);
        Task<IEnumerable<UserDto>> GetUsersAsync();
    }
}
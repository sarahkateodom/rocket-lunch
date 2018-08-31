using System.Collections.Generic;
using System.Threading.Tasks;
using makelunch.domain.dtos;

namespace makelunch.domain.contracts
{
    public interface IRepository
    {
        Task<int> CreateUserAsync(string name, IEnumerable<string> nopes);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(int id);
        Task UpdateUserAsync(int id, string name, IEnumerable<string> nopes);
    }
}
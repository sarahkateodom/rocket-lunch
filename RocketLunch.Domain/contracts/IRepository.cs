using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.contracts
{
    public interface IRepository
    {
        Task<int> CreateUserAsync(string name, IEnumerable<string> nopes);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(int id);
        Task UpdateUserAsync(int id, string name, IEnumerable<string> nopes);
    }
}
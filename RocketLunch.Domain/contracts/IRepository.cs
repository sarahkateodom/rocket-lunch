using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.contracts
{
    public interface IRepository
    {
        Task<UserDto> CreateUserAsync(string googleId, string email, string name, string photoUrl);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserAsync(string googleId);
        Task<UserDto> GetUserAsync(int id);
        Task UpdateUserAsync(int id, string name, IEnumerable<string> nopes);
    }
}
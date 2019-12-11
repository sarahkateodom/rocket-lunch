using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;
using RocketLunch.domain.utilities;

namespace RocketLunch.domain.contracts
{
    public interface IManageUsers
    {
        Task<UserDto> LoginAsync(LoginDto userDto);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<bool> UpdateUserAsync(int userId, UserUpdateDto dto);
        Task<UserDto> GetUserAsync(int id);
    }
}
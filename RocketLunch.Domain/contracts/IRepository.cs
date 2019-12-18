using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.contracts
{
    public interface IRepository
    {
        Task<UserDto> CreateUserAsync(string googleId, string email, string name, string photoUrl);
        Task<UserWithTeamsDto> GetUserAsync(string googleId);
        Task<UserWithTeamsDto> GetUserAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(int id, string name, IEnumerable<string> nopes, string Zip);
        Task<int> CreateTeamAsync(string name, string zip);
        Task AddUserToTeamAsync(int userId, int teamId);
        Task RemoveUserFromTeamAsync(int userId, int teamId);
        Task<IEnumerable<UserDto>> GetUsersOfTeamAsync(int teamId);
        Task<bool> TeamNameExistsAsync(string teamName);
        Task<IEnumerable<string>> GetNopesAsync(IEnumerable<int> userIds);
    }
}
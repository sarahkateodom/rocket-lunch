using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.contracts
{
    public interface IManageTeams
    {
        Task<TeamDto> CreateTeamAsync(int userId, CreateTeamDto dto);
        Task<IEnumerable<UserDto>> GetUsersOfTeamAsync(int teamId);
        Task RemoveUserFromTeamAsync(int teamId, int userId);
        Task<UserDto> AddUserToTeamAsync(int teamId, string email);
    }
}
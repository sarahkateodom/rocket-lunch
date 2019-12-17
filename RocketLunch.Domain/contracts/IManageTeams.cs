using System.Threading.Tasks;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.contracts
{
    public interface IManageTeams
    {
        Task<TeamDto> CreateTeamAsync(int userId, TeamDto dto);
        Task AddUserToTeamAsync(int teamId, string email);
    }
}
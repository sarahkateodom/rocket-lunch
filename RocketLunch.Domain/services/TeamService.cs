using System;
using System.Threading.Tasks;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using RocketLunch.domain.exceptions;

namespace RocketLunch.domain.services
{
    public class TeamService : IManageTeams
    {
        IRepository repository;

        public TeamService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<TeamDto> CreateTeamAsync(int userId, TeamDto dto)
        {
            if (dto == null) throw new ArgumentNullException();
            if (await this.repository.TeamNameExistsAsync(dto.Name).ConfigureAwait(false)) throw new BadRequestException("Team name exists");
            dto.Id = await this.repository.CreateTeamAsync(dto.Name, dto.Zip).ConfigureAwait(false);
            await this.repository.AddUserToTeamAsync(userId, dto.Id).ConfigureAwait(false);
            return dto;
        }
    }
}
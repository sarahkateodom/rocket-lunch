using System;
using System.Collections.Generic;
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

        public async Task<UserDto> AddUserToTeamAsync(int teamId, string email)
        {
            var user = await this.repository.GetUserByEmailAsync(email).ConfigureAwait(false);
            if(user == null) return null;
            await this.repository.AddUserToTeamAsync(user.Id, teamId).ConfigureAwait(false);
            return user;
        }

        public async Task<TeamDto> CreateTeamAsync(int userId, CreateTeamDto dto)
        {
            if (dto == null) throw new ArgumentNullException();
            if (await this.repository.TeamNameExistsAsync(dto.Name).ConfigureAwait(false)) throw new BadRequestException("Team name exists");

            var newTeam = new TeamDto
            {
                Id = await this.repository.CreateTeamAsync(dto.Name, dto.Zip).ConfigureAwait(false),
                Name = dto.Name,
                Zip = dto.Zip,
            };
            await this.repository.AddUserToTeamAsync(userId, newTeam.Id).ConfigureAwait(false);
            return newTeam;
        }

        public async Task<IEnumerable<UserDto>> GetUsersOfTeamAsync(int teamId)
        {
            return await this.repository.GetUsersOfTeamAsync(teamId).ConfigureAwait(false) ?? throw new NotFoundException("Team not found");
        }

        public async Task RemoveUserFromTeamAsync(int teamId, int userId)
        {
            await this.repository.RemoveUserFromTeamAsync(userId, teamId).ConfigureAwait(false);
        }
    }
}
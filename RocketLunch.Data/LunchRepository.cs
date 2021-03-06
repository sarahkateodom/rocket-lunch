using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLunch.data.entities;
using RocketLunch.domain.contracts;
using RocketLunch.domain.dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace RocketLunch.data
{
    public class LunchRepository : IRepository
    {
        private LunchContext _lunchContext;
        public LunchRepository(LunchContext lunchContext)
        {
            _lunchContext = lunchContext ?? throw new ArgumentNullException("lunchContext");
        }

        public async Task AddUserToTeamAsync(int userId, int teamId)
        {
            if (await _lunchContext.UserTeams.AnyAsync(ut => ut.TeamId == teamId && ut.UserId == userId)) return;

            await _lunchContext.UserTeams.AddAsync(new UserTeamEntity
            {
                TeamId = teamId,
                UserId = userId,
            }).ConfigureAwait(false);
            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> CreateTeamAsync(string name, string zip)
        {
            var team = (await _lunchContext.Teams.AddAsync(new TeamEntity
            {
                Name = name,
                Zip = zip,
            }).ConfigureAwait(false)).Entity;

            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);

            return team.Id;
        }

        public async Task<TeamDto> GetTeamAsync(int id)
        {
           TeamEntity teamEntity = await _lunchContext.Teams.SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return MaptoTeamDto(teamEntity);
        }
        
        public async Task UpdateTeamAsync(int id, string name, string zip)
        {
            TeamEntity currentTeam = await _lunchContext.Teams.FirstOrDefaultAsync( t => t.Id == id).ConfigureAwait(false);
            currentTeam.Name = name;
            currentTeam.Zip = zip;
            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<UserDto> CreateUserAsync(string googleId, string email, string name, string photoUrl)
        {
            UserEntity newUser = (await _lunchContext.Users.AddAsync(new UserEntity
            {
                GoogleId = googleId,
                Email = email,
                Name = name,
                PhotoUrl = photoUrl,
                Nopes = "[]"
            }).ConfigureAwait(false)).Entity;

            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
            return new UserDto
            {
                Id = newUser.Id,
                Name = name,
                Email = email,
                PhotoUrl = photoUrl,
            };
        }

        public async Task<IEnumerable<string>> GetNopesAsync(IEnumerable<int> userIds)
        {
            var selectedNopes = await _lunchContext.Users.Where(x => userIds.Contains(x.Id)).Select(x => x.Nopes).ToListAsync();
            // var selectedNopes = await _lunchContext.Users.Join(userIds,
            //             users => users.Id,
            //             incoming => incoming,
            //             (users, incoming) => users.Nopes).ToListAsync().ConfigureAwait(false);
            return selectedNopes
                .SelectMany(u => JsonConvert.DeserializeObject<IEnumerable<string>>(u))
                .Distinct();
        }

        public async Task<UserWithTeamsDto> GetUserAsync(int id)
        {
            UserEntity userEntity = await _lunchContext.Users.Include(x => x.UserTeams).ThenInclude(ut => ut.Team).SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return MapToUserWithTeamsDto(userEntity);
        }

        public async Task<UserWithTeamsDto> GetUserAsync(string googleId)
        {
            _lunchContext.UserTeams.Include(x => x.Team);
            var result = await _lunchContext.Users.Include(x => x.UserTeams).ThenInclude(ut => ut.Team).SingleOrDefaultAsync(u => u.GoogleId == googleId).ConfigureAwait(false);
            return MapToUserWithTeamsDto(result);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            return MapToUserDto(await _lunchContext.Users.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false));
        }

        public async Task<IEnumerable<UserDto>> GetUsersOfTeamAsync(int teamId)
        {
            var result = await _lunchContext.UserTeams.Where(ut => ut.TeamId == teamId)
                .Select(ut => MapToUserDto(ut.User)).ToListAsync();
            return (result.Count() > 0) ? result : null;
        }

        public async Task RemoveUserFromTeamAsync(int userId, int teamId)
        {
            var userTeam = await _lunchContext.UserTeams.FirstOrDefaultAsync(x => x.UserId == userId && x.TeamId == teamId).ConfigureAwait(false);
            _lunchContext.UserTeams.Remove(userTeam);
            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> TeamNameExistsAsync(string teamName) => await _lunchContext.Teams.AnyAsync(t => t.Name == teamName);

        public async Task UpdateUserAsync(int id, string name, IEnumerable<string> nopes, string zip)
        {
            UserEntity currentUser = await _lunchContext.Users.Include(x => x.UserTeams).ThenInclude(ut => ut.Team).FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
            currentUser.Name = name;
            currentUser.Nopes = JsonConvert.SerializeObject(nopes);
            currentUser.Zip = zip;
            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static UserWithTeamsDto MapToUserWithTeamsDto(UserEntity user)
        {
            if (user == null) return null;
            return new UserWithTeamsDto
            {
                Id = user.Id,
                Name = user.Name,
                Nopes = (user.Nopes != null) ? JsonConvert.DeserializeObject<List<string>>(user.Nopes) : new List<string>(),
                Email = user.Email,
                PhotoUrl = user.PhotoUrl,
                Zip = user.Zip,
                Teams = user.UserTeams.Select(x => MaptoTeamDto(x.Team))
            };
        }

        private static UserDto MapToUserDto(UserEntity user)
        {
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Nopes = (user.Nopes != null) ? JsonConvert.DeserializeObject<List<string>>(user.Nopes) : new List<string>(),
                Email = user.Email,
                PhotoUrl = user.PhotoUrl,
                Zip = user.Zip
            };
        }

        private static TeamDto MaptoTeamDto(TeamEntity team)
        {
            if (team == null) return null;
            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                Zip = team.Zip
            };
        }

        
    }
}
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

        public async Task<UserDto> CreateUserAsync(string googleId, string email, string name)
        {
            UserEntity newUser = (await _lunchContext.Users.AddAsync(new UserEntity
            {
                GoogleId = googleId,
                Email = email,
                Name = name,
            }).ConfigureAwait(false)).Entity;

            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
            return new UserDto
            {
                Id = newUser.Id,
                Name = name,
            };
        }

        public async Task<int> CreateUserAsync_Old(string name, IEnumerable<string> nopes)
        {
            UserEntity newUser = (await _lunchContext.Users.AddAsync(new UserEntity
            {
                Name = name,
                Nopes = JsonConvert.SerializeObject(nopes),
            }).ConfigureAwait(false)).Entity;

            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
            return newUser.Id;
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            UserEntity userEntity = await _lunchContext.Users.FindAsync(id).ConfigureAwait(false);
            return userEntity != null ? new UserDto
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Nopes = JsonConvert.DeserializeObject<List<string>>(userEntity.Nopes),
            } : null;
        }

        public async Task<UserDto> GetUserAsync(string googleId)
        {
            var result = await _lunchContext.Users.SingleOrDefaultAsync(u => u.GoogleId == googleId).ConfigureAwait(false);
            return result == null ? null : new UserDto
            {
                Id = result.Id,
                Name = result.Name,
                Nopes = JsonConvert.DeserializeObject<List<string>>(result.Nopes),
            };
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            return await _lunchContext.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Nopes = JsonConvert.DeserializeObject<List<string>>(u.Nopes),
            }).ToListAsync();
        }

        public async Task UpdateUserAsync(int id, string name, IEnumerable<string> nopes)
        {
            UserEntity currentUser = await _lunchContext.Users.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
            currentUser.Name = name;
            currentUser.Nopes = JsonConvert.SerializeObject(nopes);
            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
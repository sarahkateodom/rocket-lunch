using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using makelunch.data.entities;
using makelunch.domain.contracts;
using makelunch.domain.dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace makelunch.data
{
    public class LunchRepository : IRepository
    {
        private LunchContext _lunchContext;
        public LunchRepository(LunchContext lunchContext)
        {
            _lunchContext = lunchContext ?? throw new ArgumentNullException("lunchContext");
        }

        public async Task<int> CreateUserAsync(string name, string nopes)
        {
            UserEntity newUser = (await _lunchContext.Users.AddAsync(new UserEntity
            {
                Name = name,
                Nopes = nopes,
            }).ConfigureAwait(false)).Entity;

            await _lunchContext.SaveChangesAsync().ConfigureAwait(false);
            return newUser.Id;
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
    }
}
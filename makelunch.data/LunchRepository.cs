using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using makelunch.data.entities;
using makelunch.domain.contracts;

namespace makelunch.data
{
    public class LunchRepository : IRepository
    {
        private LunchContext _lunchContext;
        public LunchRepository(LunchContext lunchContext)
        {
            _lunchContext = lunchContext ?? throw new ArgumentNullException("lunchContext");
        }

        public async Task CreateUserAsync(string name, string avatarUrl)
        {
            await _lunchContext.Users.AddAsync(new UserEntity {
                Name = name,
                AvatarUrl = avatarUrl,
            }).ConfigureAwait(false);
            await _lunchContext.SaveChangesAsync();
        }
    }
}